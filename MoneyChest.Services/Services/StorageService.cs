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
    public interface IStorageService : IBaseHistoricizedService<Storage>, IIdManageable<Storage>
    {
    }

    public class StorageService : BaseHistoricizedService<Storage>, IStorageService
    {
        public StorageService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Storage entity) => entity.UserId;

        protected override Expression<Func<Storage, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #region IIdManageable<T> implementation

        public Storage Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
