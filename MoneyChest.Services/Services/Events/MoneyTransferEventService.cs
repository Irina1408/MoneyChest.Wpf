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
    public interface IMoneyTransferEventService : IBaseHistoricizedService<MoneyTransferEvent>, IIdManageable<MoneyTransferEvent>
    {
    }

    public class MoneyTransferEventService : BaseHistoricizedService<MoneyTransferEvent>, IMoneyTransferEventService
    {
        public MoneyTransferEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(MoneyTransferEvent entity) => entity.UserId;

        protected override Expression<Func<MoneyTransferEvent, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #region IIdManageable<T> implementation

        public MoneyTransferEvent Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
