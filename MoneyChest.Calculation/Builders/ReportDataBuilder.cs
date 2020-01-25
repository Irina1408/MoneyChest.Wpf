using MoneyChest.Calculation.Common;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Report;
using MoneyChest.Model.Utils;
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

        private ReportBuildSettings _prevReportSettings;
        private List<CategoryLevelMapping> _categoryLevelMapping;
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
            _categoryLevelMapping = new List<CategoryLevelMapping>();
        }

        #endregion

        #region Public methods & properties

        public string NoneCategoryName { get; set; }

        public Dictionary<ReportDataType, string> ReportDataTypeDescriptions { get; set; }

        public ReportResult Build(ReportBuildSettings settings, bool force = false)
        {
            // load data that doen't depend of settings
            LoadBaseReportData();
            // load data correpont to provided settings
            LoadData(settings, force);

            // filter transactions by data type
            var filteredTransactions = _transactions.Where(x => 
                (settings.DataType == ReportDataType.All && (x.IsExpense || x.IsIncome))
                || (settings.DataType == ReportDataType.Expense && x.IsExpense) 
                || (settings.DataType == ReportDataType.Income && x.IsIncome));

            // filter transactions by user data filter
            filteredTransactions = settings.ApplyFilter(filteredTransactions);

            return BuildReportResult(filteredTransactions, settings);
        }

        #endregion

        #region Data loading

        private void LoadBaseReportData()
        {
            // TODO: reload only if main currency or currency exchange rate was changed
            _data.MainCurrency = _currencyService.GetMain(_userId).ToReferenceView();
            _data.Rates = _currencyExchangeRateService.GetList(_userId, _data.MainCurrency.Id);

            // TODO: reload categories only when hierarchy is changed
            _data.Categories = _categoryService.GetListForUser(_userId);
        }

        private void LoadData(ReportBuildSettings settings, bool force)
        {
            if(settings.Section == BarChartSection.Category)
            {
                // set category level mapping
                MakeSureCategoryMapping(settings.CategoryLevel, force);

                // set category mapping for detailing
                for (int i = 1; i <= settings.DetailsDepth; i++)
                    MakeSureCategoryMapping(settings.CategoryLevel + i, force);
            }

            // load transactions
            if (_prevReportSettings == null || force ||
                _prevReportSettings.DateFrom != settings.DateFrom || _prevReportSettings.DateUntil != settings.DateUntil)
            {
                // cleanup existing transactions list
                _transactions = new List<ITransaction>();

                // load actual transactions
                if (settings.IncludeActualTransactions) _transactions.AddRange(
                    _transactionService.GetActual(_userId, settings.DateFrom, settings.DateUntil).Where(x => x.TransactionAmount != 0));

                // load planned future transactions
                if (settings.IncludeFuturePlannedTransactions) _transactions.AddRange(
                    _transactionService.GetPlanned(_userId, settings.DateFrom, settings.DateUntil).Where(x => x.TransactionAmount != 0));
            }

            // save settings
            _prevReportSettings = settings;
        }

        private void MakeSureCategoryMapping(int categoryLevel, bool force)
        {
            if (!_categoryLevelMapping.Any(x => x.CategoryLevel == categoryLevel))
            {
                _categoryLevelMapping.Add(new CategoryLevelMapping()
                {
                    CategoryLevel = categoryLevel,
                    CategoryMapping = _categoryService.GetCategoryMapping(_userId, categoryLevel)
                });
            }
            else if (force)
                _categoryLevelMapping.First(x => x.CategoryLevel == categoryLevel).CategoryMapping = 
                    _categoryService.GetCategoryMapping(_userId, categoryLevel);
        }

        #endregion

        #region Build

        private ReportResult BuildReportResult(IEnumerable<ITransaction> transactions, ReportBuildSettings settings)
        {
            // if combine incomes and expenses
            if (settings.DataType == ReportDataType.All)
            {
                // populate report units correspont to requested Section
                if (settings.Section == BarChartSection.Category)
                    return BuildReportResultAllDataType(transactions, settings.CategoryLevel, settings.DetailsDepth, settings.Sorting);
                else
                    return BuildReportResultAllDataType(transactions, settings.PeriodType, settings.DateFrom, settings.DateUntil);
            }
            else
            {
                var result = new ReportResult();

                // populate report units correspont to requested Section
                if (settings.Section == BarChartSection.Category)
                    result.ReportUnits = BuildReportUnits(transactions, settings.CategoryLevel, settings.DetailsDepth, settings.Sorting);
                else
                    result.ReportUnits = BuildReportUnits(transactions, settings.PeriodType, settings.DateFrom, settings.DateUntil);

                // update total amount
                result.TotAmountDetailed = FormatMainCurrency(result.ReportUnits.Sum(x => x.Amount), true);

                return result;
            }
        }

        private ReportResult BuildReportResultAllDataType(IEnumerable<ITransaction> transactions, 
            int categoryLevel, int detailsDepth, Sorting sorting)
        {
            var result = new ReportResult() { ReportUnits = new List<ReportUnit>() };
            var sbTotalAmount = new StringBuilder();

            // build incomes if any
            var incomeTransactions = transactions.Where(x => x.IsIncome).ToList();
            if (incomeTransactions.Any())
            {
                // build incomes report unit
                var reportUnit = BuildReportUnit(incomeTransactions, null, ReportDataTypeDescriptions[ReportDataType.Income], categoryLevel - 1, detailsDepth, sorting);
                reportUnit.Color = ReportUnitColor.Green;

                // update total incomes
                sbTotalAmount.Append(FormatMainCurrency(reportUnit.Amount, true, true));
                // add report unit to the result
                result.ReportUnits.Add(reportUnit);
            }

            // build expenses if any
            var expenseTransactions = transactions.Where(x => x.IsExpense).ToList();
            if (expenseTransactions.Any())
            {
                // build expenses report unit
                var reportUnit = BuildReportUnit(expenseTransactions, null, ReportDataTypeDescriptions[ReportDataType.Expense], categoryLevel - 1, detailsDepth, sorting);
                reportUnit.Color = ReportUnitColor.Red;

                // update total expenses
                if (sbTotalAmount.Length > 0) sbTotalAmount.AppendLine();
                sbTotalAmount.Append(" " + FormatMainCurrency(-reportUnit.Amount, true, true));
                // add report unit to the result
                result.ReportUnits.Add(reportUnit);
            }

            // sort result report units
            result.ReportUnits = ApplySorting(result.ReportUnits, sorting).ToList();
            // update total amount
            result.TotAmountDetailed = sbTotalAmount.ToString();

            return result;
        }

        private ReportResult BuildReportResultAllDataType(IEnumerable<ITransaction> transactions,
            PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new ReportResult() { ReportUnits = new List<ReportUnit>() };
            var periods = PeriodUtils.SplitDateRange(periodType, dateFrom, dateUntil);

            foreach (var period in periods)
            {
                // get transactions for the correspond date range
                var periodTransactions = transactions
                    .Where(x => x.TransactionDate >= period.DateFrom && x.TransactionDate <= period.DateUntil);

                // create incomes report unit
                var incomesReportUnit = new ReportUnit
                {
                    Caption = ReportDataTypeDescriptions[ReportDataType.Income],
                    Amount = Math.Abs(periodTransactions.Where(x => x.IsIncome).Sum(_ => ToMainCurrency(_.TransactionAmount, _.TransactionCurrencyId))),
                    Color = ReportUnitColor.Green
                };
                // create expense report unit
                var expensesReportUnit = new ReportUnit
                {
                    Caption = ReportDataTypeDescriptions[ReportDataType.Expense],
                    Amount = Math.Abs(periodTransactions.Where(x => x.IsExpense).Sum(_ => ToMainCurrency(_.TransactionAmount, _.TransactionCurrencyId))),
                    Color = ReportUnitColor.Red
                };

                result.ReportUnits.Add(new ReportUnit
                {
                    Caption = PeriodUtils.GetPeriodRangeDetails(periodType, period.DateFrom, period.DateUntil),
                    Detailing = new List<ReportUnit>() { incomesReportUnit, expensesReportUnit }
                });
            }

            return result;
        }

        private ReportUnit BuildReportUnit(IEnumerable<ITransaction> transactions, int? categoryId, string caption,
            int categoryLevel, int detailsDepth, Sorting sorting)
        {
            // create root item
            var root = new ReportUnit
            {
                CategoryId = categoryId,
                Caption = caption,
                Amount = Math.Abs(transactions.Sum(_ => ToMainCurrency(_.TransactionAmount, _.TransactionCurrencyId)))
            };

            // add child items
            if (detailsDepth > 0)
                root.Detailing = BuildReportUnits(transactions, categoryLevel + 1, detailsDepth - 1, sorting);

            return root;
        }

        private List<ReportUnit> BuildReportUnits(IEnumerable<ITransaction> transactions, int categoryLevel, int detailsDepth, 
            Sorting sorting)
        {
            var result = new List<ReportUnit>();

            foreach (var t in GetCategorizedTransactions(transactions, categoryLevel))
            {
                // create report unit
                var reportUnit = BuildReportUnit(t.Transactions, t.CategoryId, 
                    _data.Categories.FirstOrDefault(_ => _.Id == t.CategoryId)?.Name,
                    categoryLevel, detailsDepth, sorting);

                // add report unit to result list
                result.Add(reportUnit);
            }

            var sortedResult = ApplySorting(result, sorting).ToList();
            // fix caption for empty category after sorting (empty category should be first/last if alphabetical sorting)
            sortedResult.Where(x => x.CategoryId == null).ToList().ForEach(x => x.Caption = NoneCategoryName);

            return sortedResult;
        }

        private List<ReportUnit> BuildReportUnits(IEnumerable<ITransaction> transactions, PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<ReportUnit>();
            var periods = PeriodUtils.SplitDateRange(periodType, dateFrom, dateUntil);

            foreach (var period in periods)
            {
                // get transactions for the correspond date range
                var periodTransactions = transactions
                    .Where(x => x.TransactionDate >= period.DateFrom && x.TransactionDate <= period.DateUntil);

                // create report unit and add it to result list
                result.Add(new ReportUnit
                {
                    Caption = PeriodUtils.GetPeriodRangeDetails(periodType, period.DateFrom, period.DateUntil),
                    Amount = Math.Abs(periodTransactions.Sum(_ => ToMainCurrency(_.TransactionAmount, _.TransactionCurrencyId)))
                });
            }

            return result;
        }

        private IEnumerable<CategoryTransactions> GetCategorizedTransactions(IEnumerable<ITransaction> transactions, int categoryLevel)
        {
            var categoryMapping = _categoryLevelMapping.FirstOrDefault(x => x.CategoryLevel == categoryLevel)?.CategoryMapping;

            return transactions
                // apply transaction-category mapping
                .Select(x => new
                {
                    Transaction = x,
                    CategoryId = (categoryMapping != null && x.TransactionCategory != null && categoryMapping.ContainsKey(x.TransactionCategory.Id) ? (int?)categoryMapping[x.TransactionCategory.Id] : null)
                })
                // group by category
                .GroupBy(x => x.CategoryId)
                .Select(x => new CategoryTransactions
                {
                    CategoryId = x.Key,
                    Transactions = x.Select(_ => _.Transaction).ToList()
                })
                .ToList();
        }

        private IEnumerable<ReportUnit> ApplySorting(IEnumerable<ReportUnit> items, Sorting sorting)
        {
            switch (sorting)
            {
                case Sorting.AlphabeticalAscending:
                    return items.OrderBy(x => x.Caption);

                case Sorting.AlphabeticalDescending:
                    return items.OrderByDescending(x => x.Caption);

                case Sorting.AmountAscending:
                    return items.OrderBy(x => x.Amount);

                case Sorting.AmountDescending:
                    return items.OrderByDescending(x => x.Amount);

                default:
                    return items;
            }
        }

        #endregion

        #region Utils

        private decimal ToMainCurrency(decimal val, int currencyId) =>
            val != 0 ? CalculationHelper.ConvertToCurrency(val, currencyId, _data.MainCurrency.Id, _data.Rates) : 0;

        private string FormatMainCurrency(decimal val, bool hideZero = false, bool showSign = false) =>
            hideZero && val == 0 ? null : _data.MainCurrency.FormatValue(val, showSign);

        private class CategoryLevelMapping
        {
            public int CategoryLevel { get; set; }
            public Dictionary<int, int> CategoryMapping { get; set; }
        }

        private class CategoryTransactions
        {
            public int? CategoryId { get; set; }
            public IEnumerable<ITransaction> Transactions { get; set; }
        }

        #endregion
    }
}
