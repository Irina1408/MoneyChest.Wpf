﻿using MoneyChest.Data.Entities;
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
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services.Events
{
    public interface IRepayDebtEventService : IBaseIdManagableUserableListService<RepayDebtEventModel>
    {
    }

    public class RepayDebtEventService : BaseHistoricizedIdManageableUserableListService<RepayDebtEvent, RepayDebtEventModel, RepayDebtEventConverter>, IRepayDebtEventService
    {
        public RepayDebtEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<RepayDebtEvent> Scope => Entities.Include(_ => _.Storage).Include(_ => _.Debt);
    }
}
