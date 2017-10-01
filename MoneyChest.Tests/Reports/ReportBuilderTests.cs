using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MoneyChest.Calculation.Calculators;
using MoneyChest.Data.Mock;
using MoneyChest.Services.Services;
using MoneyChest.Data.Enums;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Settings;
using MoneyChest.Calculation.Reports;
using System.Diagnostics;

namespace MoneyChest.Tests.Reports
{
    [TestClass]
    public class ReportBuilderTests : UserableIntegrationTestBase
    {
        [TestMethod]
        public void ItBuildsReportForAllCategoriesAllLevels()
        {
            // get report settings
            var reportSettingsService = new ReportSettingService(App.Db);
            var reportSettings = reportSettingsService.GetForUser(user.Id);

            reportSettings.AllCategories = true;
            reportSettings.CategoryLevel = -1;

            // build report and check
            BuildAndCheckReport(reportSettings);
        }

        [TestMethod]
        public void ItBuildsReportForAllCategoriesSpecialLevel()
        {
            // get report settings
            var reportSettingsService = new ReportSettingService(App.Db);
            var reportSettings = reportSettingsService.GetForUser(user.Id);

            // create categories of different levels
            var categoryLvl0 = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var categoryLvl1 = App.Factory.Create<Category>(c =>
            {
                c.UserId = user.Id;
                c.ParentCategoryId = categoryLvl0.Id;
            });
            var categoryLvl2 = App.Factory.Create<Category>(c =>
            {
                c.UserId = user.Id;
                c.ParentCategoryId = categoryLvl1.Id;
            });

            reportSettings.AllCategories = true;
            reportSettings.CategoryLevel = 1;

            // build report and check
            BuildAndCheckReport(reportSettings, _ => _.CategoryId = categoryLvl0.Id, _ => _.CategoryId = categoryLvl1.Id);
        }

        [TestMethod]
        public void ItBuildsReportForSpecialCategoriesAllLevels()
        {
            // get report settings
            var reportSettingsService = new ReportSettingService(App.Db);
            var reportSettings = reportSettingsService.GetForUser(user.Id);

            // create different categories
            var category1 = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var category2 = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var category3 = App.Factory.Create<Category>(_ => _.UserId = user.Id);

            reportSettings.Categories.Add(category1);
            reportSettings.Categories.Add(category2);
            reportSettings.AllCategories = false;
            reportSettings.CategoryLevel = -1;

            // build report and check
            BuildAndCheckReport(reportSettings, _ => _.CategoryId = category2.Id, _ => _.CategoryId = category3.Id);
        }

        [TestMethod]
        public void ItBuildsReportForSpecialCategoriesSpecialLevel()
        {
            // get report settings
            var reportSettingsService = new ReportSettingService(App.Db);
            var reportSettings = reportSettingsService.GetForUser(user.Id);

            // create categories of different levels
            var categoryLvl0 = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var categoryLvl1 = App.Factory.Create<Category>(c =>
            {
                c.UserId = user.Id;
                c.ParentCategoryId = categoryLvl0.Id;
            });
            var categoryLvl2_1 = App.Factory.Create<Category>(c =>
            {
                c.UserId = user.Id;
                c.ParentCategoryId = categoryLvl1.Id;
            });
            var categoryLvl2_2 = App.Factory.Create<Category>(c =>
            {
                c.UserId = user.Id;
                c.ParentCategoryId = categoryLvl1.Id;
            });

            reportSettings.Categories.Add(categoryLvl1);
            reportSettings.Categories.Add(categoryLvl2_1);
            reportSettings.AllCategories = false;
            reportSettings.CategoryLevel = 1;

            // build report and check
            BuildAndCheckReport(reportSettings, _ => _.CategoryId = categoryLvl2_1.Id, _ => _.CategoryId = categoryLvl2_2.Id);
        }

        [TestMethod]
        public void ItBuildsReportWithCurrencyExchangeRate()
        {
            // get report settings
            var reportSettingsService = new ReportSettingService(App.Db);
            var reportSettings = reportSettingsService.GetForUser(user.Id);
            
            // get main currency
            var currencyService = new CurrencyService(App.Db);
            var mainCurrency = currencyService.GetMain(user.Id);

            // create currency exchange rate
            var currency = App.Factory.Create<Currency>(item => item.UserId = user.Id);
            var storageGroup = App.Factory.Create<StorageGroup>(item => item.UserId = user.Id);
            var storage = App.Factory.Create<Storage>(item =>
            {
                item.UserId = user.Id;
                item.CurrencyId = currency.Id;
                item.StorageGroupId = storageGroup.Id;
            });
            var currencyExchangeRate = App.Factory.Create<CurrencyExchangeRate>(item =>
            {
                item.CurrencyFromId = currency.Id;
                item.CurrencyToId = mainCurrency.Id;
            }); 
            
            reportSettings.AllCategories = true;
            reportSettings.CategoryLevel = -1;

            // build report and check
            BuildAndCheckReport(reportSettings, record => 
            {
                record.CurrencyId = currency.Id;
                record.StorageId = storage.Id;
            });
        }

        #region Private methods

        /// <summary>
        /// It builds and checks report for all combinations of TransactionType, PeriodFilterType, IncludeRecordsWithoutCategory
        /// </summary>
        private void BuildAndCheckReport(
            ReportSetting reportSettings, 
            Action<Record> createRecordOneTypeOverrides = null,
            Action<Record> createRecordSecondTypeOverrides = null)
        {
            var peridFilterTypes = Enum.GetValues(typeof(PeriodFilterType));
            var transactionTypes = Enum.GetValues(typeof(TransactionType));
            
            foreach(bool includeEmptyCategoryRecords in new[] { true, false })
            {
                foreach (TransactionType transactionType in transactionTypes)
                {
                    foreach (PeriodFilterType periodFilterType in peridFilterTypes)
                    {
                        // get correspond period
                        var period = GetPeriod(periodFilterType);
                        // create records for report of one type
                        App.Factory.CreateRecords(user.Id, period.Item1, period.Item2, record =>
                        {
                            record.TransactionType = transactionType;
                            createRecordOneTypeOverrides?.Invoke(record);
                        });
                        // create records for report of second type
                        App.Factory.CreateRecords(user.Id, period.Item1, period.Item2, record =>
                        {
                            record.TransactionType = transactionType;
                            createRecordSecondTypeOverrides?.Invoke(record);
                        });
                        // create records without category for report
                        App.Factory.CreateRecords(user.Id, period.Item1, period.Item2, record =>
                        {
                            record.TransactionType = transactionType;
                            record.CategoryId = null;
                        });
                        // update settings
                        reportSettings.IncludeRecordsWithoutCategory = includeEmptyCategoryRecords;
                        reportSettings.DataType = transactionType;
                        reportSettings.DateFrom = period.Item1;
                        reportSettings.DateUntil = period.Item2;
                        reportSettings.PeriodFilterType = periodFilterType;

                        CheckReportFetch(reportSettings);

                        Debug.WriteLine(string.Format("Success. EmptyCategoryRecords: {0}, TransactionType: {1}, PeriodFilterType: {2}",
                            includeEmptyCategoryRecords, transactionType.ToString(), periodFilterType.ToString()));
                    }
                }
            }
        }
        
        private Tuple<DateTime, DateTime> GetPeriod(PeriodFilterType period)
        {
            var settingsService = new GeneralSettingService(App.Db);
            var firstDayOfWeek = settingsService.GetForUser(user.Id).FirstDayOfWeek;

            DateTime from, until;
            int tmp1, tmp2;

            switch (period)
            {
                case PeriodFilterType.Today:
                    from = DateTime.Today;
                    until = DateTime.Today.AddDays(1).AddMinutes(-1);
                    break;

                case PeriodFilterType.Yesterday:
                    from = DateTime.Today.AddDays(-1);
                    until = DateTime.Today.AddMinutes(-1);
                    break;

                case PeriodFilterType.ThisWeek:
                    tmp1 = (int)DateTime.Today.DayOfWeek;
                    tmp2 = 0;
                    while (tmp1 != (int)firstDayOfWeek)
                    {
                        if (tmp1 == 0) tmp1 = (int)DayOfWeek.Saturday; else tmp1--;
                        tmp2++;
                    }

                    from = DateTime.Today.AddDays(-tmp2);
                    until = DateTime.Today.AddDays(7 - tmp2).AddMinutes(-1);
                    break;

                case PeriodFilterType.PreviousWeek:
                    tmp1 = (int)DateTime.Today.DayOfWeek;
                    tmp2 = 0;
                    while (tmp1 != (int)firstDayOfWeek)
                    {
                        if (tmp1 == 0) tmp1 = (int)DayOfWeek.Saturday; else tmp1--;
                        tmp2++;
                    }

                    from = DateTime.Today.AddDays(-tmp2 - 7);
                    until = DateTime.Today.AddDays(-tmp2).AddMinutes(-1);
                    break;

                case PeriodFilterType.ThisMonth:
                    from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    until = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).AddDays(1).AddMinutes(-1);
                    break;

                case PeriodFilterType.PreviousMonth:
                    tmp1 = DateTime.Today.Month > 1 ? DateTime.Today.Year : DateTime.Today.Year - 1;
                    tmp2 = DateTime.Today.Month > 1 ? DateTime.Today.Month - 1 : 12;

                    from = new DateTime(tmp1, tmp2, 1);
                    until = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMinutes(-1);
                    break;

                case PeriodFilterType.ThisYear:
                    from = new DateTime(DateTime.Today.Year, 1, 1);
                    until = new DateTime(DateTime.Today.Year + 1, 1, 1).AddMinutes(-1);
                    break;

                case PeriodFilterType.PreviousYear:
                    from = new DateTime(DateTime.Today.Year - 1, 1, 1);
                    until = new DateTime(DateTime.Today.Year, 1, 1).AddMinutes(-1);
                    break;

                default:
                    var rand = new Random();
                    from = DateTime.Today.AddDays(-rand.Next(365));
                    until = DateTime.Today.AddDays(rand.Next(365));
                    break;
            }

            return new Tuple<DateTime, DateTime>(from, until);
        }
        
        private void CheckReportFetch(ReportSetting reportSettings)
        {
            // necessary services
            var recordService = new RecordService(App.Db);
            var categoryService = new CategoryService(App.Db);
            var currencyService = new CurrencyService(App.Db);
            var currencyExchangeRateService = new CurrencyExchangeRateService(App.Db);

            // necessary values
            var mainCurrency = currencyService.GetMain(user.Id);
            mainCurrency.Should().NotBeNull();
            var currencyExchangeRates = currencyExchangeRateService.GetAllForUser(user.Id, _ => _.CurrencyToId == mainCurrency.Id);

            // initialize report instance
            var reportBuilder = new ReportBuilder(user.Id, recordService, categoryService, currencyService, currencyExchangeRateService);

            // build report
            var result = reportBuilder.BuildReport(reportSettings);

            // get all correspond records
            var records = GetRecordsToCheck(reportSettings);
            var sumByRecords = records.Sum(_ => _.Value);
            // 1. check result value
            result.Sum(_ => _.Value).ShouldBeEquivalentTo(GetSumValue(records, currencyExchangeRates, mainCurrency));

            // 2. check records without category
            if (reportSettings.IncludeRecordsWithoutCategory)
            {
                if (records.Any(_ => !_.CategoryId.HasValue))
                {
                    var reportUnit = result.FirstOrDefault(_ => _.Caption == null);
                    reportUnit.Should().NotBeNull();
                    reportUnit.Value.ShouldBeEquivalentTo(GetSumValue(records, currencyExchangeRates, mainCurrency, (int?)null));
                }
            }
            else
                result.FirstOrDefault(_ => _.Caption == null).Should().BeNull();

            // load categories
            var categoryIds = reportSettings.Categories.Select(_ => _.Id).ToList();
            var categories = reportSettings.AllCategories
                ? categoryService.GetAllForUser(user.Id)
                : categoryService.GetAllForUser(user.Id, _ => categoryIds.Contains(_.Id));

            // 3. check categories are shown when all levels are included
            if (reportSettings.CategoryLevel == -1)
            {
                categories.ForEach(category =>
                {
                    if (records.Any(_ => _.CategoryId == category.Id))
                    {
                        // take into account fact that categories can have equal values
                        var reportUnits = result.Where(_ => _.Caption == category.Name).ToList();
                        reportUnits.Count.Should().BeGreaterThan(0);
                        var value = GetSumValue(records, currencyExchangeRates, mainCurrency, category.Id);
                        reportUnits.Any(_ => _.Value == value).ShouldBeEquivalentTo(true);
                    }
                });
            }

            // 4. check categories are shown when special category level is selected
            if (reportSettings.CategoryLevel != -1)
            {
                var categoryLevelMapping = categoryService.GetCategoryLevelMapping(user.Id);
                var categoryMapping = categoryService.GetCategoryMapping(user.Id, reportSettings.CategoryLevel);

                categories.Where(_ => categoryLevelMapping[_.Id] == reportSettings.CategoryLevel).ToList().ForEach(category =>
                {
                    // get all child categories
                    var childCatIds = categoryMapping.Where(_ => _.Value == category.Id).Select(_ => _.Key);
                    // get child categories from selected by user
                    var childCategoryIds = categories.Where(_ => childCatIds.Contains(_.Id)).Select(_ => _.Id).ToList();

                    if (records.Any(_ => _.CategoryId.HasValue && childCategoryIds.Contains(_.CategoryId.Value)))
                    {
                        // take into account fact that categories can have equal values
                        var reportUnits = result.Where(_ => _.Caption == category.Name).ToList();
                        reportUnits.Count.Should().BeGreaterThan(0);
                        var value = GetSumValue(records, currencyExchangeRates, mainCurrency, childCategoryIds);
                        reportUnits.Any(_ => _.Value == value).ShouldBeEquivalentTo(true);
                    }
                });
            }
        }

        private List<Record> GetRecordsToCheck(ReportSetting reportSettings)
        {
            var recordService = new RecordService(App.Db);

            // get all records for user
            // Note: use period (DateFrom - DateUntil) from ReportSetting because IN TESTS it should correspond to PeriodFilterType
            var records = reportSettings.PeriodFilterType == PeriodFilterType.All
                ? recordService.GetAllForUser(user.Id, _ => _.TransactionType == reportSettings.DataType)
                : recordService.GetAllForUser(user.Id, _ => 
                    _.Date >= reportSettings.DateFrom.Value && _.Date <= reportSettings.DateUntil.Value 
                    && _.TransactionType == reportSettings.DataType);

            // get records that should be included into report
            List<Record> recordsToCheck = null;
            if (reportSettings.AllCategories)
            {
                // take into account IncludeRecordsWithoutCategory
                recordsToCheck = reportSettings.IncludeRecordsWithoutCategory
                    ? records
                    : records.Where(_ => _.CategoryId.HasValue).ToList();
            }
            else
            {
                var categoryIds = reportSettings.Categories.Select(_ => _.Id).ToList();
                recordsToCheck = reportSettings.IncludeRecordsWithoutCategory
                    ? records.Where(_ => !_.CategoryId.HasValue || categoryIds.Contains(_.CategoryId.Value)).ToList()
                    : records.Where(_ => _.CategoryId.HasValue && categoryIds.Contains(_.CategoryId.Value)).ToList();
            }

            return recordsToCheck;
        }

        private decimal GetSumValue(List<Record> records, List<CurrencyExchangeRate> currencyExchangeRates, 
            Currency mainCurrency, int? categoryId) => 
            GetSumValue(records, currencyExchangeRates, mainCurrency, new List<int?>() { categoryId });

        private decimal GetSumValue(List<Record> records, List<CurrencyExchangeRate> currencyExchangeRates,
            Currency mainCurrency, List<int> categoryIds) =>
            GetSumValue(records, currencyExchangeRates, mainCurrency, categoryIds.Select(_ => (int?) _).ToList());

        private decimal GetSumValue(List<Record> records, List<CurrencyExchangeRate> currencyExchangeRates,
            Currency mainCurrency, List<int?> categoryIds = null)
        {
            var recordsToBeUsed = categoryIds == null ? records : records.Where(_ => categoryIds.Contains(_.CategoryId));
            var value = recordsToBeUsed.Where(_ => _.CurrencyId == mainCurrency.Id).Sum(_ => _.Value);

            // take into account currency exchage rate if currency is not main
            recordsToBeUsed.Where(_ => _.CurrencyId != mainCurrency.Id).ToList().ForEach(item =>
            {
                var exchangeRate = currencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == item.CurrencyId);
                value += exchangeRate != null ? exchangeRate.Rate * item.Value : item.Value;
            });

            return value;
        }

        #endregion
    }
}
