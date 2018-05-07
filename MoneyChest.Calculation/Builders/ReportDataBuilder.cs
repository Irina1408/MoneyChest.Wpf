using MoneyChest.Calculation.Common;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Report;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders
{
    public class ReportDataBuilder
    {
        #region Private fields

        private int _userId;
        private ITransactionService _transactionService;
        private ICurrencyService _currencyService;
        private ICurrencyExchangeRateService _currencyExchangeRateService;
        private ICategoryService _categoryService;
        private ReportData _data;

        private ReportSettings _prevReportSettings;
        private Dictionary<int, int> _categoryMapping;
        private List<ITransaction> _transactions;

        #endregion

        #region Initialization

        public ReportDataBuilder(int userId,
            ITransactionService transactionService,
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService,
            ICategoryService categoryService)
        {
            _userId = userId;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _categoryService = categoryService;
            _data = new ReportData();
        }

        #endregion

        #region Public methods

        public ReportResult Build(DateTime dateFrom, DateTime dateUntil, int categoryLevel, RecordType dataType, bool force = false)
        {
            // prepare
            LoadBaseReportData();

            // set category level mapping
            if(_prevReportSettings == null || _prevReportSettings.CategoryLevel != categoryLevel)
                _categoryMapping = _categoryService.GetCategoryMapping(_userId, categoryLevel);

            // load transactions
            if(_prevReportSettings == null || _prevReportSettings.DateFrom != dateFrom || _prevReportSettings.DateUntil != dateUntil)
                _transactions = _transactionService.GetActual(_userId, dateFrom, dateUntil);
            
            // save settings
            if (_prevReportSettings == null)
                _prevReportSettings = new ReportSettings();
            _prevReportSettings.DateFrom = dateFrom;
            _prevReportSettings.DateUntil = dateUntil;
            _prevReportSettings.CategoryLevel = categoryLevel;
            _prevReportSettings.DataType = dataType;

            // build result
            var result = new ReportResult();
            // set helper reference
            result.ReportData = _data;
            // set transactions for selected period
            result.Transactions = _transactions.Where(x => x.TransactionAmount != 0).ToList();
            // for now define filtered transactions as all transactions
            result.FilteredTransactions = result.Transactions;

            // build result
            result.ReportUnits = result.FilteredTransactions
                .Where(x => (dataType == RecordType.Expense && x.IsExpense) || (dataType == RecordType.Income && x.IsIncome))
                .Select(x => new { Transaction = x, CategoryId = (_categoryMapping != null && x.TransactionCategory != null && _categoryMapping.ContainsKey(x.TransactionCategory.Id) ? (int?)_categoryMapping[x.TransactionCategory.Id] : null) })
                .GroupBy(x => x.CategoryId)
                .Select(x => new { CategoryId = x.Key, Amount = x.Sum(_ => ToMainCurrency(_.Transaction.TransactionAmount, _.Transaction.TransactionCurrencyId)) })
                .Select(x => new ReportUnit(_data.Categories.FirstOrDefault(_ => _.Id == x.CategoryId)?.Name, Math.Abs(x.Amount)))
                .OrderByDescending(x => x.Amount)
                .ToList();

            return result;
        }

        #endregion

        #region Private methods
        
        private void LoadBaseReportData()
        {
            // TODO: reload only if main currency or currency exchange rate was changed
            _data.MainCurrency = _currencyService.GetMain(_userId).ToReferenceView();
            _data.Rates = _currencyExchangeRateService.GetList(_userId, _data.MainCurrency.Id);

            // TODO: reload categories only when hierarchy is changed
            _data.Categories = _categoryService.GetListForUser(_userId);
        }

        private decimal ToMainCurrency(decimal val, int currencyId) =>
            val != 0 ? CalculationHelper.ConvertToCurrency(val, currencyId, _data.MainCurrency.Id, _data.Rates) : 0;

        #endregion
    }

    public class ReportSettings
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public int CategoryLevel { get; set; }
        public RecordType DataType { get; set; }
    }
}
