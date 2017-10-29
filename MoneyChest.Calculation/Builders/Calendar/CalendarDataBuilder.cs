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
            IMoneyTransferService moneyTransferService,
            IScheduleService scheduleService,
            IEventService eventService) 
            : base(userId, recordService, currencyService, currencyExchangeRateService)
        {
            _storageService = storageService;
            _moneyTransferService = moneyTransferService;
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

            // TODO: calcaulate balance changes between today and dateFrom and between dateUntil and today

            // loop for every day
            DateTime currDate = settings.From.Date;
            while (currDate <= settings.Until)
            {
                // create calendar day data object
                var calendarDayData = new CalendarDayData(currDate.Day, currDate.Month, currDate.Year);
                // fill existing records and money transfers in this day
                FillRecordsLegend(calendarDayData, currDate);
                FillMoneyTransfersLegend(calendarDayData, currDate);

                // if current day is future day fill events
                if(DateTime.Today < currDate)
                    FillEventLegend(calendarDayData, currDate);

                result.Add(calendarDayData);
                currDate = currDate.AddDays(1);
            }

            return result;
        }

        protected override void LoadData()
        {
            base.LoadData();

            _preloadedData.Currencies = _currencyService.GetUsed(_userId);
        }

        #endregion

        #region Private methods

        private void PreloadData(CalendarBuilderSettings settings)
        {
            // TODO: preload data
            /*
            // calculate current storage summary
            _preloadedData.StorageSummary = _storageSummaryCalculator.CalculateSingleCurrencySummary(_mainCurrency, 
                _currencyExchangeRates, settings.StorageGroupIds).Sum(_ => _.Value);

            // preload data from database
            _preloadedData.MoneyTransfers = _moneyTransferService.GetAfterDate(_userId, settings.From, settings.StorageGroupIds)
                .Where(item => settings.StorageGroupIds.Contains(item.StorageFrom.StorageGroupId) || settings.StorageGroupIds.Contains(item.StorageTo.StorageGroupId)).ToList();
            _preloadedData.Records = _recordService.Get(_userId, settings.From, DateTime.Today.AddDays(1), settings.StorageGroupIds);
            _preloadedData.Schedules = _scheduleService.GetActiveForPeriod(_userId, DateTime.Today, settings.Until);
            _preloadedData.Events = _eventService.Get(_preloadedData.Schedules.GetEventIds());
            */
        }

        private void FillRecordsLegend(CalendarDayData data, DateTime date)
        {
            // get records for the correspond day
            var records = _preloadedData.Records.Where(item 
                => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth);

            // fill records full info
            foreach (var record in records)
            {
                data.RecordLegend.Add(new RecordLegendUnit()
                {
                    Description = record.Description,
                    Value = record.Value,
                    Category = record?.Category,
                    Currency = record.Currency,
                    Storage = record?.Storage,
                    TransactionType = record.TransactionType,
                    IsPlanned = false
                });
            }
        }

        private void FillMoneyTransfersLegend(CalendarDayData data, DateTime date)
        {
            // get money transfers for the correspond day
            var moneyTransfers = _preloadedData.MoneyTransfers.Where(item
                => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth).ToList();

            // write full info of money transfers
            foreach (var moneyTransfer in moneyTransfers)
            {
                data.MoneyTransferLegend.Add(new MoneyTransferLegendUnit
                {
                    StorageFrom = moneyTransfer.StorageFrom,
                    StorageTo = moneyTransfer.StorageTo,
                    ValueCurrencyFrom = new ValueUnit(moneyTransfer.StorageFromCurrency, moneyTransfer.Value),
                    ValueCurrencyTo = new ValueUnit(moneyTransfer.StorageToCurrency, moneyTransfer.Value * moneyTransfer.CurrencyExchangeRate),
                    Category = moneyTransfer?.Category,
                    Description = moneyTransfer.Description,
                    TakeComissionFromReceiver = moneyTransfer.TakeComissionFromReceiver,
                    Comission = new ValueUnit(moneyTransfer.StorageFromCurrency, moneyTransfer.CommisionValue),
                    IsPlanned = false
                });
            }
        }

        private void FillEventLegend(CalendarDayData data, DateTime date)
        {
            // TODO: write events in order
            // write every month events
            foreach (var schedule in _preloadedData.Schedules.MonthlySchedules.Where(e => e.DayOfMonth == date.Day
                && e.Months.Contains((Month)date.Month)).Distinct())
            {
                // write event
                WriteEvent(data, schedule.EventId);
            }

            // write every week events
            foreach (var schedule in _preloadedData.Schedules.WeeklySchedules.Where(item => item.DaysOfWeek.Contains(date.DayOfWeek)))
            {
                // write event
                WriteEvent(data, schedule.EventId);
            }

            // write every day events
            foreach (var schedule in _preloadedData.Schedules.DailySchedules.Where(item => item.Period > 0))
            {
                // check this event will in this day
                DateTime evntDate = schedule.DateFrom;
                while (evntDate < date)
                    evntDate = evntDate.AddDays(schedule.Period);
                if (evntDate != date) continue;

                // write event
                WriteEvent(data, schedule.EventId);
            }

            // write once events
            foreach (var schedule in _preloadedData.Schedules.OnceSchedules.Where(item => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth))
            {
                // write event
                WriteEvent(data, schedule.EventId);
            }
        }

        private void WriteEvent(CalendarDayData data, int eventId)
        {
            // get event
            var eventType = _preloadedData.Events.GetEventType(eventId);

            if(eventType == EventType.Simple)
            {
                // simple event data fill
                var evnt = _preloadedData.Events.SimpleEvents.First(_ => _.Id == eventId);
                
                data.RecordLegend.Add(new RecordLegendUnit()
                {
                    Description = evnt.Description,
                    Value = evnt.Value,
                    Category = evnt?.Category,
                    Currency = evnt.Currency,
                    Storage = evnt.Storage,
                    TransactionType = evnt.TransactionType,
                    IsPlanned = true
                });
            }

            if(eventType == EventType.RepayDebt)
            {
                // repay debt event data fill
                var evnt = _preloadedData.Events.RepayDebtEvents.First(_ => _.Id == eventId);

                data.RecordLegend.Add(new RecordLegendUnit()
                {
                    Description = evnt.Description,
                    Value = evnt.Value,
                    Category = evnt?.DebtCategory,
                    Currency = evnt.Currency,
                    Storage = evnt.Storage,
                    TransactionType = evnt.Debt.DebtType == DebtType.TakeBorrow ? TransactionType.Expense : TransactionType.Income,
                    IsPlanned = true
                });
            }

            if(eventType == EventType.MoneyTransfer)
            {
                // money transfer event data fill
                var evnt = _preloadedData.Events.MoneyTransferEvents.First(_ => _.Id == eventId);

                data.MoneyTransferLegend.Add(new MoneyTransferLegendUnit
                {
                    StorageFrom = evnt.StorageFrom,
                    StorageTo = evnt.StorageTo,
                    ValueCurrencyFrom = new ValueUnit(evnt.StorageFromCurrency, evnt.Value),
                    ValueCurrencyTo = new ValueUnit(evnt.StorageToCurrency, evnt.Value * evnt.CurrencyExchangeRate),
                    Category = evnt?.Category,
                    Description = evnt.Description,
                    TakeComissionFromReceiver = evnt.TakeComissionFromReceiver,
                    Comission = new ValueUnit(evnt.StorageFromCurrency, evnt.CommisionValue),
                    IsPlanned = true
                });
            }
        }

        #endregion

        #region Helpers

        private class PreloadedData
        {
            public List<CurrencyModel> Currencies { get; set; }

            public decimal StorageSummary { get; set; }
            public List<RecordModel> Records { get; set; }
            public List<MoneyTransferModel> MoneyTransfers { get; set; }
            public SchedulesScopeModel Schedules { get; set; }
            public EventsScopeModel Events { get; set; }
        }

        #endregion
    }
}
