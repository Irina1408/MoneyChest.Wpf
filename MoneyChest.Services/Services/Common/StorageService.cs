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
using MoneyChest.Services.Converters;

namespace MoneyChest.Services.Services
{
    public interface IStorageService : IIdManagableUserableListServiceBase<StorageModel>
    {
        List<StorageModel> GetList(int userId, List<int> storageGroupIds);
        List<StorageModel> GetVisible(int userId, params int[] requiredIds);
    }

    public class StorageService : HistoricizedIdManageableUserableListServiceBase<Storage, StorageModel, StorageConverter>, IStorageService
    {
        public StorageService(ApplicationDbContext context) : base(context)
        {
        }

        #region IStorageService implementation

        public List<StorageModel> GetList(int userId, List<int> storageGroupIds)
        {
            return Scope.Where(e => e.UserId == userId && storageGroupIds.Contains(e.StorageGroupId)).ToList().ConvertAll(_converter.ToModel);
        }

        public List<StorageModel> GetVisible(int userId, params int[] requiredIds)
        {
            var ids = requiredIds?.ToList() ?? new List<int>();
            return Scope.Where(e => e.UserId == userId && (e.IsVisible || ids.Contains(e.Id))).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<Storage> Scope => Entities.Include(_ => _.Currency).Include(_ => _.StorageGroup);

        #endregion
    }
}
