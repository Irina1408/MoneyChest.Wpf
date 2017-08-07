using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services
{
    public interface IStorageGroupService : IBaseHistoricizedService<StorageGroup>, IIdManageable<StorageGroup>
    {
    }

    public class StorageGroupService : BaseHistoricizedService<StorageGroup>, IStorageGroupService
    {
        public StorageGroupService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(StorageGroup entity) => entity.UserId;

        protected override Expression<Func<StorageGroup, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #region IIdManageable<T> implementation

        public StorageGroup Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
