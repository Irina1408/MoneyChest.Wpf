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
        List<RecordModel> Get(int userId, PeriodFilterType period, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null);
        List<RecordModel> Get(int userId, DateTime from, DateTime until, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null);

        List<RecordModel> Get(int userId, DateTime from, DateTime until);

        RecordModel Create(SimpleEventModel model);
        RecordModel Create(RepayDebtEventModel model);
    }

    public class RecordService : HistoricizedIdManageableUserableListServiceBase<Record, RecordModel, RecordConverter>, IRecordService
    {
        #region Initialization

        public RecordService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region IRecordService implementation

        public List<RecordModel> Get(int userId, PeriodFilterType period, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if (period == PeriodFilterType.All || period == PeriodFilterType.CustomPeriod)
                // all records for transaction type and categories
                if(categoryIds == null)
                    return Scope.Where(item => item.UserId == userId && item.RecordType == recordType
                         && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null)).ToList().ConvertAll(_converter.ToModel);
                else
                    return Scope.Where(item => item.UserId == userId && item.RecordType == recordType
                         && (includeWithoutCategory && item.CategoryId == null
                        || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId)))).ToList().ConvertAll(_converter.ToModel);
            else
            {
                // get general settings for getting first day of week
                var generalSettings = _context.GeneralSettings.FirstOrDefault(item => item.UserId == userId);
                // get period
                var p = ServiceHelper.GetPeriod(period, generalSettings.FirstDayOfWeek);
                // return result
                return Get(userId, p.Item1, p.Item2, recordType, includeWithoutCategory, categoryIds);
            }
        }

        public List<RecordModel> Get(int userId, DateTime from, DateTime until, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            if(categoryIds == null)
                return Scope.Where(item => item.UserId == userId && item.RecordType == recordType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null || item.CategoryId != null)).ToList().ConvertAll(_converter.ToModel);
            else
                return Scope.Where(item => item.UserId == userId && item.RecordType == recordType
                    && item.Date >= from && item.Date <= until
                    && (includeWithoutCategory && item.CategoryId == null
                    || (item.CategoryId != null && categoryIds.Contains((int)item.CategoryId)))).ToList().ConvertAll(_converter.ToModel);
        }

        public List<RecordModel> Get(int userId, DateTime from, DateTime until)
        {
            return Scope.Where(item => item.UserId == userId && item.Date >= from && item.Date <= until)
                .ToList().ConvertAll(_converter.ToModel);
        }

        public RecordModel Create(SimpleEventModel model)
        {
            return new RecordModel()
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
                Category = model.Category
            };
        }

        public RecordModel Create(RepayDebtEventModel model)
        {
            var debt = _context.Debts.FirstOrDefault(x => x.Id == model.DebtId);

            return new RecordModel()
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
                Category = model.DebtCategory
            };
        }

        #endregion

        #region Overrides

        public override RecordModel Add(RecordModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

            return base.Add(model);
        }

        public override IEnumerable<RecordModel> Add(IEnumerable<RecordModel> models)
        {
            var categoryIds = models.Where(x => x.CategoryId != null).Select(x => x.CategoryId).Distinct().ToList();
            var categories = _context.Categories.Where(x => categoryIds.Contains(x.Id));

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
            {
                var category = categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

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
            AddValueToStorage(model.StorageId, model.ResultValueSignExchangeRate);

            // update related debt
            if (model.DebtId.HasValue)
                AddValueToDebt(model.DebtId.Value, model.ResultValueExchangeRate);

            // save changes
            SaveChanges();
        }

        public override void OnUpdated(RecordModel oldModel, RecordModel model)
        {
            base.OnUpdated(oldModel, model);

            // update related storage
            if (oldModel.StorageId != model.StorageId)
            {
                // remove value from old storage
                AddValueToStorage(oldModel.StorageId, -oldModel.ResultValueSignExchangeRate);

                // add value to the new storage
                AddValueToStorage(model.StorageId, model.ResultValueSignExchangeRate);
            }
            else if(oldModel.ResultValueSignExchangeRate != model.ResultValueSignExchangeRate)
                AddValueToStorage(model.StorageId, model.ResultValueSignExchangeRate - oldModel.ResultValueSignExchangeRate);

            // update related debt
            if (oldModel.DebtId != model.DebtId)
            {
                // remove value from old debt
                if (oldModel.DebtId.HasValue)
                    AddValueToDebt(oldModel.DebtId.Value, -oldModel.ResultValueExchangeRate);

                // add value to the new debt
                if (model.DebtId.HasValue)
                    AddValueToDebt(model.DebtId.Value, model.ResultValueExchangeRate);
            }
            else if (oldModel.ResultValueExchangeRate != model.ResultValueExchangeRate && model.DebtId.HasValue)
                AddValueToDebt(model.DebtId.Value, model.ResultValueExchangeRate - oldModel.ResultValueExchangeRate);

            // save changes
            SaveChanges();
        }

        public override void OnDeleted(RecordModel model)
        {
            base.OnDeleted(model);

            // update related storage
            AddValueToStorage(model.StorageId, -model.ResultValueSignExchangeRate);

            // update related debt
            if (model.DebtId.HasValue)
                AddValueToDebt(model.DebtId.Value, -model.ResultValueExchangeRate);

            // save changes
            SaveChanges();
        }

        protected override IQueryable<Record> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.Debt);

        #endregion

        #region Private methods

        private void AddValueToStorage(int storageId, decimal value)
        {
            var storage = _context.Storages.FirstOrDefault(_ => _.Id == storageId);
            storage.Value += value;
            _historyService.WriteHistory(storage, ActionType.Update, storage.UserId);
        }

        private void AddValueToDebt(int debtId, decimal value)
        {
            var debtConverter = new DebtConverter();
            var debt = _context.Debts.Include(_ => _.DebtPenalties).FirstOrDefault(_ => _.Id == debtId);
            debt.PaidValue += value;
            debt.IsRepaid = debtConverter.ToModel(debt).RemainsToPay <= 0;
            _historyService.WriteHistory(debt, ActionType.Update, debt.UserId);
        }

        #endregion
    }
}
