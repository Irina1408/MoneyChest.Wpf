using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferService : IBaseHistoricizedService<MoneyTransfer>
    {
    }

    public class MoneyTransferService : BaseHistoricizedService<MoneyTransfer>, IMoneyTransferService
    {
        public MoneyTransferService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(MoneyTransfer entity)
        {
            if (entity.StorageFrom != null) return entity.StorageFrom.UserId;
            if (entity.StorageTo != null) return entity.StorageTo.UserId;
            return _context.Currencies.FirstOrDefault(item => item.Id == entity.StorageFromId).UserId;
        }

        protected override Func<MoneyTransfer, bool> LimitByUser(int userId) => 
            item => item.StorageFrom.UserId == userId && item.StorageTo.UserId == userId;
    }
}
