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
using MoneyChest.Model.Extensions;
using MoneyChest.Services.Converters;
using MoneyChest.Data.Enums;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferService : IIdManagableServiceBase<MoneyTransferModel>, IUserableListService<MoneyTransferModel>
    {
        List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, List<int> storageGroupIds);
        List<MoneyTransferModel> GetAfterDate(int userId, DateTime date, List<int> storageGroupIds);
    }

    public class MoneyTransferService : HistoricizedIdManageableServiceBase<MoneyTransfer, MoneyTransferModel, MoneyTransferConverter>, IMoneyTransferService
    {
        public MoneyTransferService(ApplicationDbContext context) : base(context)
        {
        }

        #region IMoneyTransferService implementation

        public List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, List<int> storageGroupIds)
        {
            return Scope.Where(item => item.StorageFrom.UserId == userId && item.Date >= from && item.Date <= until
                    //&& (item.StorageFrom.StorageGroupId != item.StorageTo.StorageGroupId || item.StorageFrom.CurrencyId != item.StorageTo.CurrencyId)
                    && (storageGroupIds.Contains(item.StorageFromId) || storageGroupIds.Contains(item.StorageToId)))
                    .ToList().ConvertAll(_converter.ToModel);
        }

        public List<MoneyTransferModel> GetAfterDate(int userId, DateTime date, List<int> storageGroupIds)
        {
            return Scope.Where(item => item.StorageFrom.UserId == userId && item.Date >= date
                    //&& (item.StorageFrom.StorageGroupId != item.StorageTo.StorageGroupId || item.StorageFrom.CurrencyId != item.StorageTo.CurrencyId)
                    && (storageGroupIds.Contains(item.StorageFromId) || storageGroupIds.Contains(item.StorageToId)))
                    .ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region IUserableListService<MoneyTransferModel> implementation

        public List<MoneyTransferModel> GetListForUser(int userId) =>
            Scope.Where(item => item.StorageFrom.UserId == userId && item.StorageTo.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        #endregion

        #region Overrides

        public override void OnAdded(MoneyTransferModel model, MoneyTransfer entity)
        {
            // update related storages
            var storageFrom = _context.Storages.FirstOrDefault(_ => _.Id == model.StorageFromId);
            storageFrom.Value -= model.StorageFromValue;
            _historyService.WriteHistory(storageFrom, ActionType.Update, storageFrom.UserId);

            var storageTo = _context.Storages.FirstOrDefault(_ => _.Id == model.StorageToId);
            storageTo.Value += model.StorageToValue;
            _historyService.WriteHistory(storageTo, ActionType.Update, storageTo.UserId);

            // save changes
            SaveChanges();
        }

        public override void OnUpdated(MoneyTransferModel oldModel, MoneyTransferModel model)
        {
            // update related storages
            if(oldModel.StorageFromValue != model.StorageFromValue)
            {
                var storageFrom = _context.Storages.FirstOrDefault(_ => _.Id == model.StorageFromId);
                storageFrom.Value += oldModel.StorageFromValue - model.StorageFromValue;
                _historyService.WriteHistory(storageFrom, ActionType.Update, storageFrom.UserId);
            }

            if(oldModel.StorageToValue != model.StorageToValue)
            {
                var storageTo = _context.Storages.FirstOrDefault(_ => _.Id == model.StorageToId);
                storageTo.Value -= oldModel.StorageToValue - model.StorageToValue;
                _historyService.WriteHistory(storageTo, ActionType.Update, storageTo.UserId);
            }
                
            // save changes
            SaveChanges();
        }

        public override void OnDeleted(MoneyTransferModel model)
        {
            // update related storages
            var storageFrom = _context.Storages.FirstOrDefault(_ => _.Id == model.StorageFromId);
            storageFrom.Value += model.StorageFromValue;
            _historyService.WriteHistory(storageFrom, ActionType.Update, storageFrom.UserId);

            var storageTo = _context.Storages.FirstOrDefault(_ => _.Id == model.StorageToId);
            storageTo.Value -= model.StorageToValue;
            _historyService.WriteHistory(storageTo, ActionType.Update, storageTo.UserId);

            // save changes
            SaveChanges();
        }

        protected override IQueryable<MoneyTransfer> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);

        protected override int UserId(MoneyTransfer entity)
        {
            if (entity.StorageFrom != null) return entity.StorageFrom.UserId;
            if (entity.StorageTo != null) return entity.StorageTo.UserId;
            return _context.Storages.FirstOrDefault(item => item.Id == entity.StorageFromId).UserId;
        }

        #endregion
    }
}
