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

        public List<RepayDebtEventModel> GetNotClosed(int userId)
        {
            return Scope.Where(x => x.EventState != Model.Enums.EventState.Closed && x.UserId == userId).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        public override RepayDebtEventModel Add(RepayDebtEventModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                var debt = _context.Debts.Include(_ => _.Category).FirstOrDefault(x => x.Id == model.DebtId);
                model.Description = debt.Category?.Name;
            }

            return base.Add(model);
        }

        public override IEnumerable<RepayDebtEventModel> Add(IEnumerable<RepayDebtEventModel> models)
        {
            var debtIds = models.Select(x => x.DebtId).Distinct().ToList();
            var debts = _context.Debts.Include(_ => _.Category).Where(x => debtIds.Contains(x.Id));

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
            {
                var debt = debts.FirstOrDefault(x => x.Id == model.DebtId);
                model.Description = debt.Category?.Name;
            }

            return base.Add(models);
        }

        protected override IQueryable<RepayDebtEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Debt.Currency).Include(_ => _.Debt.Category);
    }
}
