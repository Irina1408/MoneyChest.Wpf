using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Events
{
    public class RepayDebtEventService : BaseHistoricizedService<RepayDebtEvent>
    {
        public RepayDebtEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(RepayDebtEvent entity) => entity.UserId;
    }
}
