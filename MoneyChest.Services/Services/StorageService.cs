using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services
{
    public interface IStorageService : IBaseHistoricizedService<Storage>
    {
    }

    public class StorageService : BaseHistoricizedService<Storage>, IStorageService
    {
        public StorageService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Storage entity) => entity.UserId;

        protected override Expression<Func<Storage, bool>> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
