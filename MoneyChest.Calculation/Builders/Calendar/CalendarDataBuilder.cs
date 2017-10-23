using MoneyChest.Calculation.Builders.Base;
using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services;
using MoneyChest.Calculation.Calculators;
using MoneyChest.Calculation.Common;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Services.Events;
using MoneyChest.Calculation.Summary;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Model;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class CalendarDataBuilder : DataBuilderBase<CalendarBuilderSettings, CalendarDayData>
    {
        #region Private fields

        private IStorageService _storageService;
        private IMoneyTransferService _moneyTransferService;
        private IRepayDebtEventService _repayDebtEventService;
        private ISimpleEventService _simpleEventService;
        private IScheduleService _scheduleService;
        private IEventService _eventService;

        private StorageSummaryCalculator _storageSummaryCalculator;
        private PreloadedData _preloadedData;

        #endregion

        #region Initialization

        public CalendarDataBuilder(int userId, 
            IRecordService recordService, 
            ICurrencyService currencyService, 
            ICurrencyExchangeRateService currencyExchangeRateService,
            IStorageService storageService,
            IRepayDebtEventService repayDebtEventService,
            ISimpleEventService simpleEventService,
            IMoneyTransferService moneyTransferService,
            IScheduleService scheduleService,
            IEventService eventService) 
            : base(userId, recordService, currencyService, currencyExchangeRateService)
        {
            _storageService = storageService;
            _moneyTransferService = moneyTransferService;
            _repayDebtEventService = repayDebtEventService;
            _simpleEventService = simpleEventService;
            _scheduleService = scheduleService;
            _eventService = eventService;
            _storageSummaryCalculator = new StorageSummaryCalculator(_storageService, _userId);
            _preloadedData = new PreloadedData();
        }

        #endregion

        #region Overrides

        protected override List<CalendarDayData> BuildResult(CalendarBuilderSettings settings)
        {
            // result
            var result = new List<CalendarDayData>();
            // preload data correspond to settings
            PreloadData(settings);

            // loop for every day
            DateTime currDate = settings.From.Date;
            while (currDate <= settings.Until)
            {
                // create calendar day data object
                var calendarDayData = new CalendarDayData(currDate.Day, currDate.Month, currDate.Year)
                {
                    // set initial balance
                    Balance = _preloadedData.StorageSummary
                };

                // fill data by records if current day is today or past day
                if (DateTime.Today >= currDate)
                    FillDataByRecords(calendarDayData, currDate, settings.StorageGroupIds);
                else
                {
                    //FillDataByEvents(calendarDayData, currDate, settings.StorageGroupIds);
                }

                result.Add(calendarDayData);
                currDate = currDate.AddDays(1);
            }

            return result;
        }

        protected override void LoadData()
        {
            base.LoadData();
        }

        #endregion

        #region Private methods

        private void PreloadData(CalendarBuilderSettings settings)
        {
            // calculate current storage summary
            _preloadedData.StorageSummary = _storageSummaryCalculator.CalculateSingleCurrencySummary(_mainCurrency, 
                _currencyExchangeRates, settings.StorageGroupIds).Sum(_ => _.Value);
            // preload records
            /*
            _preloadedData.Records = _recordService.GetListForUser(_userId, item => item.Date >= settings.From
                && (!item.StorageId.HasValue || settings.StorageGroupIds.Contains(item.StorageId.Value)));
            // preload money transfers
            _preloadedData.MoneyTransfers = _moneyTransferService.GetListForUser(_userId, item => item.Date >= settings.From
                    && item.Date <= settings.Until 
                    && (item.StorageFrom.StorageGroupId != item.StorageTo.StorageGroupId || item.StorageFrom.CurrencyId != item.StorageTo.CurrencyId)
                    && (settings.StorageGroupIds.Contains(item.StorageFromId) || settings.StorageGroupIds.Contains(item.StorageToId)));
            // preload schedules
            _preloadedData.OnceSchedules = _scheduleService.GetActiveForPeriod<OnceSchedule>(_userId, settings.From, settings.Until);
            _preloadedData.DailySchedules = _scheduleService.GetActiveForPeriod<DailySchedule>(_userId, settings.From, settings.Until);
            _preloadedData.WeeklySchedules = _scheduleService.GetActiveForPeriod<WeeklySchedule>(_userId, settings.From, settings.Until);
            _preloadedData.MonthlySchedules = _scheduleService.GetActiveForPeriod<MonthlySchedule>(_userId, settings.From, settings.Until);
            */
        }

        private void FillDataByRecords(CalendarDayData data, DateTime date, List<int> storageGroupIds)
        {
            // get records for the correspond day
            var records = _preloadedData.Records.Where(item 
                => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth);

            // calculate expences and incomes in this day by records
            foreach (var record in records)
            {
                if(record.TransactionType == TransactionType.Expense)
                {
                    data.TotalExpences += CalculationHelper.ConvertToCurrency(record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
                    data.Legend.Add(new LegendUnit(record.Currency, -record.Value, record.Description));
                }
                else
                {
                    data.TotalIncomes += CalculationHelper.ConvertToCurrency(record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
                    data.Legend.Add(new LegendUnit(record.Currency, record.Value, record.Description));
                }
            }
            
            // get all money transfers after this day between storages with differens currencies
            var moneyTransfers = _preloadedData.MoneyTransfers.Where(item => item.Date > date.AddDays(1));
            /*
            // revert all money transfers after this day between storages with differens currencies
            foreach (var moneyTransfer in moneyTransfers)
            {
                if(storageGroupIds.Contains(moneyTransfer.StorageFrom.StorageGroupId))
                    data.Balance += CalculationHelper.ConvertToCurrency(moneyTransfer.Value, moneyTransfer.StorageFrom.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
                if (storageGroupIds.Contains(moneyTransfer.StorageTo.StorageGroupId))
                    data.Balance += CalculationHelper.ConvertToCurrency(-moneyTransfer.Value * moneyTransfer.CurrencyExchangeRate, moneyTransfer.StorageFrom.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
            }
            */
            // "remove" records after this day from balance
            foreach (var record in _preloadedData.Records.Where(item => date.AddDays(1) < item.Date))
            {
                data.Balance += CalculationHelper.ConvertToCurrency(record.TransactionType == TransactionType.Expense ? record.Value : -record.Value, record.CurrencyId, _mainCurrency.Id, _currencyExchangeRates);
            }
        }

        private void FillDataByEvents(CalendarDayData data, DateTime date, List<int> storageGroupIds)
        {

        }

        private void FillLegendByEvents(CalendarDayData data, DateTime date, List<int> storageGroupIds)
        {
            //var evnts = _simpleEventService.GetAllForUser(_userId, item => item.EventState != EventState.Closed
            //               && (!item.Schedule.DateUntil.HasValue || item.Schedule.DateUntil.Value >= date)
            //               && (!item.PausedToDate.HasValue || item.PausedToDate.Value < date));

            //// write every month events
            //foreach (var evnt in evnts.Where(item => item.Schedule.ScheduleType == ScheduleType.EveryMonth
            //    && item.Schedule.DayOfMonth == date.Day && item.Schedule.Months[(Month)date.Month]))
            //{
            //    // write event data
            //    WriteEvent(data, evnt);
            //}

            /*
            foreach (var schedue in _preloadedData.MonthlySchedules.Where(e => e.DayOfMonth == date.Day
                && e.MonthlyScheduleMonths.Any(m => m.Month == (Month)date.Month)).Distinct())
            {

            }
            */

            //// write every week events
            //foreach (var evnt in evnts.Where(item => item.Schedule.ScheduleType == ScheduleType.EveryWeek
            //    && item.Schedule.DaysOfWeek[date.DayOfWeek]))
            //{
            //    // write event data
            //    WriteEvent(data, evnt);
            //}

            //// write every day events
            //foreach (var evnt in evnts.Where(item => item.Schedule.ScheduleType == ScheduleType.EveryDay
            //    && item.Schedule.DayPeriod > 0))
            //{
            //    // check this event will in this day
            //    DateTime evntDate = evnt.Schedule.DateFrom;

            //    while (evntDate < date)
            //    {
            //        evntDate = evntDate.AddDays(evnt.Schedule.DayPeriod);
            //    }

            //    if (evntDate != date) continue;

            //    // write event data
            //    WriteEvent(data, evnt);
            //}

            //// write once events
            //foreach (var evnt in evnts.Where(item => item.Schedule.ScheduleType == ScheduleType.Once
            //&& item.Schedule.DateFrom == date))
            //{
            //    // write event data
            //    WriteEvent(data, evnt);
            //}
        }

        private void WriteEvent(CalendarDayData data, Evnt evnt)
        {
            //// determine sammary
            //List<SummaryUnit> summary = evnt.TransactionType == TransactionType.Expense
            //    ? reportUnit.TotalExpences
            //    : reportUnit.TotalIncomes;

            //// get summary unit for current currency
            //var sUnit = GetSummaryUnit(summary, evnt.Currency);

            //sUnit.Value += evnt.Value;

            //reportUnit.Legend.Add(new KeyValuePair<string, SummaryUnit>(
            //    evnt.Description, new SummaryUnit(evnt.Currency)
            //    {
            //        Value = evnt.TransactionType == TransactionType.Expense ? -evnt.Value : evnt.Value
            //    }));
        }

        #endregion

        #region Helpers

        private class PreloadedData
        {
            public decimal StorageSummary { get; set; }
            public List<RecordModel> Records { get; set; }
            public List<MoneyTransferModel> MoneyTransfers { get; set; }
            public List<OnceScheduleModel> OnceSchedules { get; set; }
            public List<DailyScheduleModel> DailySchedules { get; set; }
            public List<WeeklyScheduleModel> WeeklySchedules { get; set; }
            public List<MonthlyScheduleModel> MonthlySchedules { get; set; }
        }

        #endregion
    }
}
