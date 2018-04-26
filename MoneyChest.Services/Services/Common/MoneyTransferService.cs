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
    // TODO: cleanup
    public interface IMoneyTransferService : IIdManagableServiceBase<MoneyTransferModel>, IUserableListService<MoneyTransferModel>
    {
        List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, List<int> storageGroupIds);
        List<MoneyTransferModel> GetAfterDate(int userId, DateTime date, List<int> storageGroupIds);

        List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until);

        MoneyTransferModel Create(MoneyTransferEventModel model);
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

        public List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until)
        {
            return Scope.Where(item => item.StorageFrom.UserId == userId && item.Date >= from && item.Date <= until)
                .ToList().ConvertAll(_converter.ToModel);
        }

        public MoneyTransferModel Create(MoneyTransferEventModel model)
        {
            return new MoneyTransferModel()
            {
                Date = DateTime.Now,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId,
                CategoryId = model.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Description = model.Description,
                Remark = model.Remark,
                TakeCommissionFromReceiver = model.TakeCommissionFromReceiver,
                Value = model.Value,
                StorageFrom = model.StorageFrom,
                StorageFromCurrency = model.StorageFromCurrency,
                StorageTo = model.StorageTo,
                StorageToCurrency = model.StorageToCurrency,
                StorageFromValue = model.StorageFromValue,
                StorageToValue = model.StorageToValue,
                Category = model.Category
            };
        }

        #endregion

        #region IUserableListService<MoneyTransferModel> implementation

        public List<MoneyTransferModel> GetListForUser(int userId) =>
            Scope.Where(item => item.StorageFrom.UserId == userId && item.StorageTo.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        #endregion

        #region Overrides

        public override MoneyTransferModel Add(MoneyTransferModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category.Name;
            }

            return base.Add(model);
        }

        public override IEnumerable<MoneyTransferModel> Add(IEnumerable<MoneyTransferModel> models)
        {
            var categoryIds = models.Where(x => x.CategoryId != null).Select(x => x.CategoryId).Distinct().ToList();
            var categories = _context.Categories.Where(x => categoryIds.Contains(x.Id));

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
            {
                var category = categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category.Name;
            }

            return base.Add(models);
        }

        public override void OnAdded(MoneyTransferModel model, MoneyTransfer entity)
        {
            base.OnAdded(model, entity);

            // update related storages
            AddValueToStorage(model.StorageFromId, -model.StorageFromValue);
            AddValueToStorage(model.StorageToId, model.StorageToValue);

            // save changes
            SaveChanges();
        }

        public override void OnUpdated(MoneyTransferModel oldModel, MoneyTransferModel model)
        {
            base.OnUpdated(oldModel, model);

            // update related storages
            if (oldModel.StorageFromId != model.StorageFromId)
            {
                // add value to old storage
                AddValueToStorage(oldModel.StorageFromId, oldModel.StorageFromValue);
                // remove value from new storage
                AddValueToStorage(model.StorageFromId, -model.StorageFromValue);
            }
            else if(oldModel.StorageFromValue != model.StorageFromValue)
                AddValueToStorage(model.StorageFromId, oldModel.StorageFromValue - model.StorageFromValue);
            
            if (oldModel.StorageToId != model.StorageToId)
            {
                // remove value from old storage
                AddValueToStorage(oldModel.StorageToId, -oldModel.StorageToValue);
                // add value to new storage
                AddValueToStorage(model.StorageToId, model.StorageToValue);
            }
            else if(oldModel.StorageToValue != model.StorageToValue)
                AddValueToStorage(model.StorageToId, model.StorageToValue - oldModel.StorageToValue);

            // save changes
            SaveChanges();
        }

        public override void OnDeleted(MoneyTransferModel model)
        {
            base.OnDeleted(model);

            // update related storages
            AddValueToStorage(model.StorageFromId, model.StorageFromValue);
            AddValueToStorage(model.StorageToId, -model.StorageToValue);

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

        #region Private methods

        private void AddValueToStorage(int storageId, decimal value)
        {
            var storage = _context.Storages.FirstOrDefault(_ => _.Id == storageId);
            storage.Value += value;
            _historyService.WriteHistory(storage, ActionType.Update, storage.UserId);
        }

        #endregion
    }
}
