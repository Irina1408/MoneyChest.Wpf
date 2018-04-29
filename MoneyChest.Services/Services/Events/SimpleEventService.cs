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
using MoneyChest.Data.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using MoneyChest.Model.Enums;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Services
{
    public interface ISimpleEventService : IIdManagableUserableListServiceBase<SimpleEventModel>, IEventService<SimpleEventModel>
    {
    }

    public class SimpleEventService : HistoricizedIdManageableUserableListServiceBase<SimpleEvent, SimpleEventModel, SimpleEventConverter>, ISimpleEventService
    {
        public SimpleEventService(ApplicationDbContext context) : base(context)
        {
        }

        #region ISimpleEventService implementation

        public List<SimpleEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var filter = EventService.GetActiveEventsFilter<SimpleEvent>(userId, dateFrom, dateUntil);
            return Scope.Where(filter).ToList().ConvertAll(_converter.ToModel);
        }

        public List<SimpleEventModel> GetNotClosed(int userId)
        {
            return Scope.Where(x => x.EventState != EventState.Closed && x.UserId == userId).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        public override SimpleEventModel Add(SimpleEventModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

            return base.Add(model);
        }

        public override IEnumerable<SimpleEventModel> Add(IEnumerable<SimpleEventModel> models)
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

        public override SimpleEventModel PrepareNew(SimpleEventModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default currency
            var mainCurrency = _context.Currencies.FirstOrDefault(x => x.IsMain && model.UserId == x.UserId);
            model.CurrencyId = mainCurrency?.Id ?? 0;
            model.Currency = mainCurrency?.ToReferenceView();

            // set default storage
            var storage = _context.Storages.FirstOrDefault(x => x.CurrencyId == model.CurrencyId && model.UserId == x.UserId);
            model.StorageId = storage?.Id ?? 0;
            model.Storage = storage?.ToReferenceView();

            return model;
        }

        protected override IQueryable<SimpleEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Currency).Include(_ => _.Category);

        #endregion
    }
}
