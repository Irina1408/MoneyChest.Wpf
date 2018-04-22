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
    public interface IRepayDebtEventService : IIdManagableUserableListServiceBase<RepayDebtEventModel>, IEventService<RepayDebtEventModel>
    {
    }

    public class RepayDebtEventService : HistoricizedIdManageableUserableListServiceBase<RepayDebtEvent, RepayDebtEventModel, RepayDebtEventConverter>, IRepayDebtEventService
    {
        public RepayDebtEventService(ApplicationDbContext context) : base(context)
        {
        }

        #region IRepayDebtEventService implementation

        public List<RepayDebtEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var filter = EventService.GetActiveEventsFilter<RepayDebtEvent>(userId, dateFrom, dateUntil);
            return Scope.Where(filter).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        public override RepayDebtEventModel PrepareNew(RepayDebtEventModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default storage
            model.StorageId = _context.Storages.FirstOrDefault(x => x.CurrencyId == model.Debt.CurrencyId)?.Id ?? 0;

            return model;
        }

        protected override IQueryable<RepayDebtEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Debt.Currency).Include(_ => _.Debt.Category);
    }
}
