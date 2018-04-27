using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using System.Data.Entity;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferEventService : IIdManagableUserableListServiceBase<MoneyTransferEventModel>, IEventService<MoneyTransferEventModel>
    {
    }

    public class MoneyTransferEventService : HistoricizedIdManageableUserableListServiceBase<MoneyTransferEvent, MoneyTransferEventModel, MoneyTransferEventConverter>, IMoneyTransferEventService
    {
        public MoneyTransferEventService(ApplicationDbContext context) : base(context)
        {
        }

        #region IMoneyTransferEventService implementation

        public List<MoneyTransferEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var filter = EventService.GetActiveEventsFilter<MoneyTransferEvent>(userId, dateFrom, dateUntil);
            return Scope.Where(filter).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        public override MoneyTransferEventModel Add(MoneyTransferEventModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

            return base.Add(model);
        }

        public override IEnumerable<MoneyTransferEventModel> Add(IEnumerable<MoneyTransferEventModel> models)
        {
            var categoryIds = models.Where(x => x.CategoryId != null).Select(x => x.CategoryId).Distinct().ToList();
            var categories = _context.Categories.Where(x => categoryIds.Contains(x.Id));

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
            {
                var category = categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

            return base.Add(models);
        }

        protected override IQueryable<MoneyTransferEvent> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);
    }
}
