using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services.Events
{
    public interface IMoneyTransferEventService : IBaseHistoricizedService<MoneyTransferEvent>
    {
    }

    public class MoneyTransferEventService : BaseHistoricizedService<MoneyTransferEvent>, IMoneyTransferEventService
    {
        public MoneyTransferEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(MoneyTransferEvent entity) => entity.UserId;

        protected override Expression<Func<MoneyTransferEvent, bool>> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
