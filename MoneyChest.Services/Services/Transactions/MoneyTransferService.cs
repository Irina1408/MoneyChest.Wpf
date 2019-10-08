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
using MoneyChest.Services.Utils;
using MoneyChest.Model.Enums;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferService : IIdManagableServiceBase<MoneyTransferModel>, IUserableListService<MoneyTransferModel>
    {
        List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, bool? AutoExecuted = null);

        List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null);

        MoneyTransferModel Create(MoneyTransferEventModel model, Action<MoneyTransferModel> overrides = null);

        MoneyTransferModel Duplicate(MoneyTransferModel model, Action<MoneyTransferModel> overrides = null);
    }

    public class MoneyTransferService : HistoricizedIdManageableServiceBase<MoneyTransfer, MoneyTransferModel, MoneyTransferConverter>, IMoneyTransferService
    {
        #region Private fields

        private ITransactionDependenceService _transactionDependenceService;

        #endregion

        #region Initialization

        public MoneyTransferService(ApplicationDbContext context) : base(context)
        {
            _transactionDependenceService = new TransactionDependenceService(context);
        }

        #endregion

        #region IMoneyTransferService implementation

        public List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, bool? AutoExecuted = null)
        {
            // build filter by autoexecution
            Expression<Func<MoneyTransfer, bool>> autoExecutionFilter = x => true;
            if (AutoExecuted.HasValue) autoExecutionFilter = x => x.IsAutoExecuted == AutoExecuted;

            return Scope.Where(item => item.StorageFrom.UserId == userId && item.Date >= from && item.Date <= until)
                .Where(autoExecutionFilter)
                .ToList().ConvertAll(_converter.ToModel);
        }

        public List<MoneyTransferModel> Get(int userId, DateTime from, DateTime until, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if (categoryIds == null)
                return Scope.Where(item => item.StorageFrom.UserId == userId
                        && item.Date >= from && item.Date <= until
                        && ((recordType == RecordType.Expense && item.Commission > 0)
                            || (recordType == RecordType.Income && item.Commission < 0))
                        && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null))
                    .ToList().ConvertAll(_converter.ToModel);
            else
                return Scope.Where(item => item.StorageFrom.UserId == userId
                        && item.Date >= from && item.Date <= until
                        && ((recordType == RecordType.Expense && item.Commission > 0)
                            || (recordType == RecordType.Income && item.Commission < 0))
                        && (includeWithoutCategory && item.CategoryId == null
                            || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId))))
                    .ToList().ConvertAll(_converter.ToModel);
        }

        public MoneyTransferModel Create(MoneyTransferEventModel model, Action<MoneyTransferModel> overrides = null)
        {
            var moneyTransfer = new MoneyTransferModel()
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
                Category = model.Category,
                EventId = model.Id
            };

            overrides?.Invoke(moneyTransfer);
            return moneyTransfer;
        }
        
        public MoneyTransferModel Duplicate(MoneyTransferModel model, Action<MoneyTransferModel> overrides = null)
        {
            var moneyTransfer = new MoneyTransferModel()
            {
                Date = DateTime.Now,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Value = model.Value,
                Description = model.Description,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                TakeCommissionFromReceiver = model.TakeCommissionFromReceiver,
                Remark = model.Remark,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId,
                CategoryId = model.CategoryId
            };

            overrides?.Invoke(moneyTransfer);
            return moneyTransfer;
        }

        #endregion

        #region IUserableListService<MoneyTransferModel> implementation

        public List<MoneyTransferModel> GetListForUser(int userId) =>
            Scope.Where(item => item.StorageFrom.UserId == userId && item.StorageTo.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        #endregion

        #region Overrides

        public override MoneyTransferModel Add(MoneyTransferModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            return base.Add(model);
        }

        public override IEnumerable<MoneyTransferModel> Add(IEnumerable<MoneyTransferModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            return base.Add(models);
        }

        public override void OnAdded(MoneyTransferModel model, MoneyTransfer entity)
        {
            base.OnAdded(model, entity);

            // update related storages
            _transactionDependenceService.AddValueToStorage(model.StorageFromId, -model.StorageFromValue);
            _transactionDependenceService.AddValueToStorage(model.StorageToId, model.StorageToValue);

            // update limits if there is any commission
            if (model.Commission != 0)
                _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId,
                    model.StorageFromCurrency.Id, model.StorageFromCommissionValue, model.StorageToCurrency.Id, model.StorageToCommissionValue);

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
                _transactionDependenceService.AddValueToStorage(oldModel.StorageFromId, oldModel.StorageFromValue);
                // remove value from new storage
                _transactionDependenceService.AddValueToStorage(model.StorageFromId, -model.StorageFromValue);
            }
            else if(oldModel.StorageFromValue != model.StorageFromValue)
                _transactionDependenceService.AddValueToStorage(model.StorageFromId, oldModel.StorageFromValue - model.StorageFromValue);
            
            if (oldModel.StorageToId != model.StorageToId)
            {
                // remove value from old storage
                _transactionDependenceService.AddValueToStorage(oldModel.StorageToId, -oldModel.StorageToValue);
                // add value to new storage
                _transactionDependenceService.AddValueToStorage(model.StorageToId, model.StorageToValue);
            }
            else if(oldModel.StorageToValue != model.StorageToValue)
                _transactionDependenceService.AddValueToStorage(model.StorageToId, model.StorageToValue - oldModel.StorageToValue);

            // update limits
            if (oldModel.TransactionAmount != model.TransactionAmount
                || oldModel.CategoryId != model.CategoryId || oldModel.Date != model.Date
                || oldModel.StorageFrom.CurrencyId != model.StorageFrom.CurrencyId)
            {
                // remove from limits an old spent value if there was any commission
                if (oldModel.Commission != 0)
                    _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId,
                        model.StorageFrom.CurrencyId, -model.StorageFromCommissionValue,
                        model.StorageTo.CurrencyId, -model.StorageToCommissionValue);

                // update limits if there is any commission
                if (model.Commission != 0)
                    _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId,
                        model.StorageFrom.CurrencyId, model.StorageFromCommissionValue, 
                        model.StorageTo.CurrencyId, model.StorageToCommissionValue);

            }

            // save changes
            SaveChanges();
        }

        public override void OnDeleted(MoneyTransferModel model)
        {
            base.OnDeleted(model);

            // update related storages
            _transactionDependenceService.AddValueToStorage(model.StorageFromId, model.StorageFromValue);
            _transactionDependenceService.AddValueToStorage(model.StorageToId, -model.StorageToValue);

            // update limits if there is any commission
            if (model.Commission != 0)
                _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId,
                    model.StorageFrom.CurrencyId, -model.StorageFromCommissionValue, model.StorageTo.CurrencyId, -model.StorageToCommissionValue);


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
