using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using System.Data.Entity;
using MoneyChest.Model.Model;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferService : IBaseIdManagableService<MoneyTransferModel>, IUserableListService<MoneyTransferModel>
    {
    }

    public class MoneyTransferService : BaseHistoricizedIdManageableService<MoneyTransfer, MoneyTransferModel, MoneyTransferConverter>, IMoneyTransferService
    {
        public MoneyTransferService(ApplicationDbContext context) : base(context)
        {
        }

        #region IUserableListService<MoneyTransferModel> implementation

        public List<MoneyTransferModel> GetListForUser(int userId) =>
            Scope.Where(item => item.StorageFrom.UserId == userId && item.StorageTo.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        #endregion

        #region Overrides

        protected override IQueryable<MoneyTransfer> Scope => Entities.Include(_ => _.StorageFrom).Include(_ => _.StorageTo);

        protected override int UserId(MoneyTransfer entity)
        {
            if (entity.StorageFrom != null) return entity.StorageFrom.UserId;
            if (entity.StorageTo != null) return entity.StorageTo.UserId;
            return _context.Storages.FirstOrDefault(item => item.Id == entity.StorageFromId).UserId;
        }

        #endregion
    }
}
