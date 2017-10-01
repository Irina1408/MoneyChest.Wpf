using MoneyChest.Calculation.Common;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Reports
{
    public class ReportBuilder
    {
        #region Private fields

        private int _userId;
        private IRecordService _recordService;
        private ICategoryService _categoryService;
        private ICurrencyService _currencyService;
        private ICurrencyExchangeRateService _currencyExchangeRateService;

        private bool _isDataLoaded;
        private Currency _mainCurrency;
        private List<CurrencyExchangeRate> _currencyExchangeRates;

        #endregion

        #region Initialization

        public ReportBuilder(int userId, 
            IRecordService recordService, 
            ICategoryService categoryService, 
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService)
        {
            _userId = userId;
            _recordService = recordService;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _isDataLoaded = false;
        }

        #endregion

        #region Public methods

        public IEnumerable<ReportUnit> BuildReport(ReportSetting reportSettings)
        {
            // load "static" data
            if(!_isDataLoaded) LoadData();

            // load categories
            var categoryIds = reportSettings.Categories.Select(_ => _.Id).ToList();
            var categories = reportSettings.AllCategories 
                ? _categoryService.GetAllForUser(_userId)
                : _categoryService.Get(categoryIds);

            // load records
            List<Record> records = null;
            if (reportSettings.PeriodFilterType == PeriodFilterType.CustomPeriod)
                records = _recordService.Get(_userId, reportSettings.DateFrom.Value, reportSettings.DateUntil.Value,
                    reportSettings.DataType, reportSettings.IncludeRecordsWithoutCategory, reportSettings.AllCategories ? null : categoryIds);
            else
                records = _recordService.Get(_userId, reportSettings.PeriodFilterType,
                    reportSettings.DataType, reportSettings.IncludeRecordsWithoutCategory, reportSettings.AllCategories ? null : categoryIds);

            // get category mapping
            var categoryMapping = _categoryService.GetCategoryMapping(_userId, reportSettings.CategoryLevel);
            var recordsOtherCurrency = records.Where(_ => _.CurrencyId != _mainCurrency.Id).ToList();

            // calculate values for every category
            var catValue = new Dictionary<int, decimal>();
            foreach (var record in records)
            {
                // get correspond category id from category mapping
                var catId = -1;
                if (record.CategoryId.HasValue)
                    catId = categoryMapping[record.CategoryId.Value];

                if (catValue.ContainsKey(catId))
                    catValue[catId] += CalculationHelper.ConvertToMainCurrency(record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
                else
                    catValue.Add(catId, CalculationHelper.ConvertToMainCurrency(record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates));
            }

            // build report result
            var result = catValue
                .Select(item => new ReportUnit(categories.FirstOrDefault(_ => _.Id == item.Key)?.Name, item.Value))
                .ToList();

            // update percentages
            decimal totalValue = result.Sum(item => item.Value);
            result.ForEach(item => item.Percentage = item.Value / totalValue);

            return result;
        }

        #endregion

        #region Private methods

        private void LoadData()
        {
            _mainCurrency = _currencyService.GetMain(_userId);
            _currencyExchangeRates = _currencyExchangeRateService.GetAllForUser(_userId, item => item.CurrencyTo.IsMain);
            _isDataLoaded = true;
        }

        #endregion
    }
}
