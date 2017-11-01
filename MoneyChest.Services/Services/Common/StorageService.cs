using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services
{
    public interface IStorageService : IBaseIdManagableUserableListService<StorageModel>
    {
        List<StorageModel> GetList(int userId, List<int> storageGroupIds);
    }

    public class StorageService : BaseHistoricizedIdManageableUserableListService<Storage, StorageModel, StorageConverter>, IStorageService
    {
        public StorageService(ApplicationDbContext context) : base(context)
        {
        }

        #region IStorageService implementation

        public List<StorageModel> GetList(int userId, List<int> storageGroupIds)
        {
            return Scope.Where(e => e.UserId == userId && storageGroupIds.Contains(e.StorageGroupId)).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<Storage> Scope => Entities.Include(_ => _.Currency).Include(_ => _.StorageGroup);

        #endregion
    }
}
