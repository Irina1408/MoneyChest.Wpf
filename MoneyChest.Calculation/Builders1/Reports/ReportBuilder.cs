using MoneyChest.Calculation.Builders.Base;
using MoneyChest.Calculation.Common;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Reports
{
    public class ReportBuilder : DataBuilderBase<ReportSettingModel, ReportUnit>
    {
        #region Private fields
        
        private ICategoryService _categoryService;

        #endregion

        #region Initialization

        public ReportBuilder(int userId, 
            IRecordService recordService, 
            ICategoryService categoryService, 
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService) 
            : base(userId, recordService, currencyService, currencyExchangeRateService)
        {
            _recordService = recordService;
            _categoryService = categoryService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
        }

        #endregion

        #region Overrides

        protected override List<ReportUnit> BuildResult(ReportSettingModel settings)
        {
            // load categories
            var categories = settings.AllCategories
                ? _categoryService.GetListForUser(_userId)
                : _categoryService.Get(settings.CategoryIds);

            // load records
            List<RecordModel> records = null;
            if (settings.PeriodFilterType == PeriodFilterType.CustomPeriod)
                records = _recordService.Get(_userId, settings.DateFrom.Value, settings.DateUntil.Value,
                    settings.DataType.Value, settings.IncludeRecordsWithoutCategory, settings.AllCategories ? null : settings.CategoryIds);
            else
                records = _recordService.Get(_userId, settings.PeriodFilterType,
                    settings.DataType.Value, settings.IncludeRecordsWithoutCategory, settings.AllCategories ? null : settings.CategoryIds);

            // get category mapping
            var categoryMapping = _categoryService.GetCategoryMapping(_userId, settings.CategoryLevel);

            // calculate values for every category
            var catValue = new Dictionary<int, decimal>();
            foreach (var record in records)
            {
                // get correspond category id from category mapping
                var catId = -1;
                if (record.CategoryId.HasValue)
                    catId = categoryMapping[record.CategoryId.Value];

                if (catValue.ContainsKey(catId))
                    catValue[catId] += CalculationHelper.ConvertToCurrency(record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
                else
                    catValue.Add(catId, CalculationHelper.ConvertToCurrency(record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates));
            }

            // build report result
            var result = catValue
                .Select(item => new ReportUnit(categories.FirstOrDefault(_ => _.Id == item.Key)?.Name, item.Value))
                .ToList();

            // update percentages
            decimal totalValue = result.Sum(item => item.Value);
            result.ForEach(item => item.Percentage = item.Value / totalValue);

            return new List<ReportUnit>();
        }

        #endregion
    }
}
