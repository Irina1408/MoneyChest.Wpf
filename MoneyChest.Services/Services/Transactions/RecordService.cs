using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Utils;
using MoneyChest.Model.Model;
using System.Data.Entity;
using MoneyChest.Model.Extensions;
using MoneyChest.Services.Converters;
using MoneyChest.Model.Enums;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Services
{
    public interface IRecordService : IIdManagableUserableListServiceBase<RecordModel>
    {
        List<DateTime> GetRecentDates(int userId, int count);

        List<RecordModel> Get(int userId, DateTime from, DateTime until, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null);
        List<RecordModel> Get(int userId, DateTime from, DateTime until, bool? AutoExecuted = null);

        RecordModel Create(SimpleEventModel model, Action<RecordModel> overrides = null);
        RecordModel Create(RepayDebtEventModel model, Action<RecordModel> overrides = null);

        RecordModel Duplicate(RecordModel model, Action<RecordModel> overrides = null);

        void CreateForDebt(DebtModel model);
    }

    public class RecordService : HistoricizedIdManageableUserableListServiceBase<Record, RecordModel, RecordConverter>, IRecordService
    {
        #region Private fields

        private ITransactionDependenceService _transactionDependenceService;

        #endregion

        #region Initialization

        public RecordService(ApplicationDbContext context) : base(context)
        {
            _transactionDependenceService = new TransactionDependenceService(context);
        }

        #endregion

        #region IRecordService implementation

        public List<DateTime> GetRecentDates(int userId, int count)
        {
            return Scope.Where(item => item.UserId == userId)
                .Select(item => item.Date)
                .Distinct()
                .OrderByDescending(item => item)
                .Take(count)
                .ToList();
        }

        public List<RecordModel> Get(int userId, DateTime from, DateTime until, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if (categoryIds == null)
                return Scope.Where(item => item.UserId == userId && item.RecordType == recordType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null)).ToList().ConvertAll(_converter.ToModel);
            else
                return Scope.Where(item => item.UserId == userId && item.RecordType == recordType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null
                    || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId)))).ToList().ConvertAll(_converter.ToModel);
        }

        public List<RecordModel> Get(int userId, DateTime from, DateTime until, bool? AutoExecuted = null)
        {
            Expression<Func<Record, bool>> autoExecutionFilter = x => true;
            if (AutoExecuted.HasValue) autoExecutionFilter = x => x.IsAutoExecuted == AutoExecuted;

            return Scope.Where(item => item.UserId == userId && item.Date >= from && item.Date <= until)
                .Where(autoExecutionFilter)
                .ToList().ConvertAll(_converter.ToModel);
        }

        public RecordModel Create(SimpleEventModel model, Action<RecordModel> overrides = null)
        {
            var record = new RecordModel()
            {
                Date = DateTime.Now,
                CategoryId = model.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                CurrencyId = model.CurrencyId,
                Description = model.Description,
                RecordType = model.RecordType,
                StorageId = model.StorageId,
                Value = model.Value,
                UserId = model.UserId,
                Remark = model.Remark,
                Currency = model.Currency,
                Storage = model.Storage,
                Category = model.Category,
                EventId = model.Id
            };

            overrides?.Invoke(record);
            return record;
        }

        public RecordModel Create(RepayDebtEventModel model, Action<RecordModel> overrides = null)
        {
            var debt = _context.Debts.FirstOrDefault(x => x.Id == model.DebtId);

            var record = new RecordModel()
            {
                Date = DateTime.Now,
                CategoryId = debt.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                CurrencyId = model.Currency.Id,
                Description = model.Description,
                RecordType = debt.DebtType == DebtType.TakeBorrow ? RecordType.Expense : RecordType.Income,
                StorageId = model.StorageId,
                Value = model.Value,
                UserId = model.UserId,
                Remark = model.Remark,
                DebtId = model.DebtId,
                Debt = debt.ToReferenceView(),
                Storage = model.Storage,
                Category = model.DebtCategory,
                Currency = model.Currency,
                EventId = model.Id
            };

            overrides?.Invoke(record);
            return record;
        }

        public RecordModel Duplicate(RecordModel model, Action<RecordModel> overrides = null)
        {
            var record = new RecordModel()
            {
                Date = DateTime.Now,
                Description = model.Description,
                RecordType = model.RecordType,
                Value = model.Value,
                Remark = model.Remark,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CategoryId = model?.CategoryId,
                CurrencyId = model.CurrencyId,
                Currency = model.Currency,
                StorageId = model.StorageId,
                Storage = model.Storage,
                DebtId = model?.DebtId,
                UserId = model.UserId
            };

            overrides?.Invoke(record);
            return record;
        }

        public void CreateForDebt(DebtModel model)
        {
            // only when storage is defined
            if (!model.StorageId.HasValue || model.StorageId <= 0) return;

            // value to be added to the related storage
            var valueForStorage = 0M;
            // values that should be used for updating limits
            var valueForLimits = 0M;
            var valueExchangeRateForLimits = 0M;

            // create a new record for the debt to keep the history
            if (!model.OnlyInitialFee)
            {
                var record = this.Add(CreateRecordForDebt(model, 
                    model.DebtType == DebtType.TakeBorrow ? RecordType.Income : RecordType.Expense, x => x.Value));

                valueForStorage += model.ValueExchangeRate;
                valueForLimits += record.RecordType == RecordType.Expense ? model.Value : 0;
                valueExchangeRateForLimits += record.RecordType == RecordType.Expense ? model.ValueExchangeRate : 0;
            }

            // create record for the initial fee
            if (model.InitialFee != 0)
            {
                var record = this.Add(CreateRecordForDebt(model, 
                    model.DebtType == DebtType.TakeBorrow ? RecordType.Expense : RecordType.Income, x => x.InitialFee));

                valueForStorage += -model.InitialFeeExchangeRate;
                valueForLimits += record.RecordType == RecordType.Expense ? model.InitialFee : 0;
                valueExchangeRateForLimits += record.RecordType == RecordType.Expense ? model.InitialFeeExchangeRate : 0;
            }

            // update related storage value
            if (valueForStorage != 0)
            {
                _transactionDependenceService.AddValueToStorage(model.StorageId.Value,
                    (model.DebtType == Model.Enums.DebtType.TakeBorrow ? 1 : -1) * valueForStorage);
            }

            // update limits
            if (valueForLimits != 0 || valueExchangeRateForLimits != 0)
                _transactionDependenceService.UpdateLimits(model.TakingDate, model.CategoryId,
                    model.CurrencyId, valueForLimits, model.Storage?.CurrencyId, valueExchangeRateForLimits);
        }

        #endregion

        #region Overrides

        public override RecordModel Add(RecordModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            return base.Add(model);
        }

        public override IEnumerable<RecordModel> Add(IEnumerable<RecordModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            return base.Add(models);
        }

        public override RecordModel PrepareNew(RecordModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default currency
            var mainCurrency = _context.Currencies.FirstOrDefault(x => x.IsMain && model.UserId == x.UserId);
            model.CurrencyId = mainCurrency?.Id ?? 0;
            model.Currency = mainCurrency?.ToReferenceView();

            // set default storage
            var storage = _context.Storages.FirstOrDefault(x => x.CurrencyId == model.CurrencyId && model.UserId == x.UserId);
            model.StorageId = storage?.Id ?? 0;
            model.Storage = storage?.ToReferenceView();

            return model;
        }

        public override void OnAdded(RecordModel model, Record entity)
        {
            base.OnAdded(model, entity);

            // update related storage
            _transactionDependenceService.AddValueToStorage(model.StorageId, model.ResultValueSignExchangeRate);

            // update related debt
            if (model.DebtId.HasValue)
                _transactionDependenceService.AddValueToDebt(model.DebtId.Value, model.ResultValueExchangeRate);

            // update limits
            if (model.RecordType == RecordType.Expense)
                _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId, 
                    model.CurrencyId, model.ResultValue, model.CurrencyIdForRate, model.ResultValueExchangeRate);

            // save changes
            SaveChanges();
            // save dependence history
            _transactionDependenceService.SaveChanges();
        }

        public override void OnUpdated(RecordModel oldModel, RecordModel model)
        {
            base.OnUpdated(oldModel, model);

            // update related storage
            if (oldModel.StorageId != model.StorageId)
            {
                // remove value from old storage
                _transactionDependenceService.AddValueToStorage(oldModel.StorageId, -oldModel.ResultValueSignExchangeRate);
                // add value to the new storage
                _transactionDependenceService.AddValueToStorage(model.StorageId, model.ResultValueSignExchangeRate);
            }
            else if (oldModel.ResultValueSignExchangeRate != model.ResultValueSignExchangeRate)
                _transactionDependenceService.AddValueToStorage(model.StorageId, model.ResultValueSignExchangeRate - oldModel.ResultValueSignExchangeRate);


            // update related debt
            if (oldModel.DebtId != model.DebtId)
            {
                // remove value from old debt
                if (oldModel.DebtId.HasValue)
                    _transactionDependenceService.AddValueToDebt(oldModel.DebtId.Value, -oldModel.ResultValueExchangeRate);
                // add value to the new debt
                if (model.DebtId.HasValue)
                    _transactionDependenceService.AddValueToDebt(model.DebtId.Value, model.ResultValueExchangeRate);
            }
            else if (oldModel.ResultValueExchangeRate != model.ResultValueExchangeRate && model.DebtId.HasValue)
                _transactionDependenceService.AddValueToDebt(model.DebtId.Value, model.ResultValueExchangeRate - oldModel.ResultValueExchangeRate);

            // update limits
            if (oldModel.RecordType != model.RecordType 
                || oldModel.CategoryId != model.CategoryId || oldModel.Date != model.Date
                || oldModel.CurrencyId != model.CurrencyId || oldModel.CurrencyIdForRate != model.CurrencyIdForRate
                || oldModel.ResultValue != model.ResultValue || oldModel.ResultValueExchangeRate != model.ResultValueExchangeRate)
            {
                // remove from limits an old spent value
                _transactionDependenceService.UpdateLimits(oldModel.Date, oldModel.CategoryId, 
                    oldModel.CurrencyId, -oldModel.ResultValue, oldModel.CurrencyIdForRate, -oldModel.ResultValueExchangeRate);

                // if record is expence update limits spent value
                if (model.RecordType == RecordType.Expense)
                    _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId, 
                        model.CurrencyId, model.ResultValue, model.CurrencyIdForRate, model.ResultValueExchangeRate);
            }

            // save changes
            SaveChanges();
            // save dependence history
            _transactionDependenceService.SaveChanges();
        }

        public override void OnDeleted(RecordModel model)
        {
            base.OnDeleted(model);

            // update related storage
            _transactionDependenceService.AddValueToStorage(model.StorageId, -model.ResultValueSignExchangeRate);

            // update related debt
            if (model.DebtId.HasValue)
                _transactionDependenceService.AddValueToDebt(model.DebtId.Value, -model.ResultValueExchangeRate);

            // update limits
            if (model.RecordType == RecordType.Expense)
                _transactionDependenceService.UpdateLimits(model.Date, model.CategoryId, 
                    model.CurrencyId, -model.ResultValue, model.CurrencyIdForRate, -model.ResultValueExchangeRate);

            // save changes
            SaveChanges();
            // save dependence history
            _transactionDependenceService.SaveChanges();
        }

        protected override IQueryable<Record> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.Debt);

        #endregion

        #region Private methods

        private Record CreateRecordForDebt(DebtModel model, RecordType recordType, Func<DebtModel, decimal> value)
        {
            return new Record()
            {
                Date = model.TakingDate,
                Description = model.Description,
                CategoryId = model.CategoryId,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                CurrencyId = model.CurrencyId,
                DebtId = model.Id,
                RecordType = recordType,
                StorageId = model.StorageId.Value,
                UserId = model.UserId,
                Value = value(model),
                Remark = model.Remark
            };
        }

        #endregion
    }
}
