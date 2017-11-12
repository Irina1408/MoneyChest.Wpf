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

namespace MoneyChest.Services.Services.Events
{
    public interface IRepayDebtEventService : IIdManagableUserableListServiceBase<RepayDebtEventModel>
    {
    }

    public class RepayDebtEventService : HistoricizedIdManageableUserableListServiceBase<RepayDebtEvent, RepayDebtEventModel, RepayDebtEventConverter>, IRepayDebtEventService
    {
        public RepayDebtEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<RepayDebtEvent> Scope => Entities.Include(_ => _.Storage).Include(_ => _.Debt.Currency).Include(_ => _.Debt.Category);
    }
}
