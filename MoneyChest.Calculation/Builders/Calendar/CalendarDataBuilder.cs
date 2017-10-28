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

                // TODO: if today add records and events
                // fill data by records if current day is today or past day
                if (DateTime.Today >= currDate)
                {
                    FillRecordsLegend(calendarDayData, currDate);
                    FillMoneyTransfersLegend(calendarDayData, currDate, settings.StorageGroupIds);
                }
                else
                {
                    FillEventLegend(calendarDayData, currDate, settings.StorageGroupIds);
                }

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
            // calculate current storage summary
            _preloadedData.StorageSummary = _storageSummaryCalculator.CalculateSingleCurrencySummary(_mainCurrency, 
                _currencyExchangeRates, settings.StorageGroupIds).Sum(_ => _.Value);

            // preload data from database
            _preloadedData.MoneyTransfers = _moneyTransferService.GetAfterDate(_userId, settings.From, settings.StorageGroupIds)
                .Where(item => settings.StorageGroupIds.Contains(item.StorageFrom.StorageGroupId) || settings.StorageGroupIds.Contains(item.StorageTo.StorageGroupId)).ToList();
            _preloadedData.Records = _recordService.Get(_userId, settings.From, DateTime.Today.AddDays(1), settings.StorageGroupIds);
            _preloadedData.Schedules = _scheduleService.GetActiveForPeriod(_userId, DateTime.Today, settings.Until);
            _preloadedData.Events = _eventService.Get(_preloadedData.Schedules.GetEventIds());
        }

        private void FillRecordsLegend(CalendarDayData data, DateTime date)
        {
            // get records for the correspond day
            var records = _preloadedData.Records.Where(item 
                => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth);

            // calculate expences and incomes in this day by records
            foreach (var record in records)
            {
                if(record.TransactionType == TransactionType.Expense)
                {
                    data.TotalExpences += ConvertToMainCurrency(record.Value, record.CurrencyId);
                    data.Legend.Add(new LegendUnit(record.Currency, -record.Value, record.Description));
                }
                else
                {
                    data.TotalIncomes += ConvertToMainCurrency(record.Value, record.CurrencyId);
                    data.Legend.Add(new LegendUnit(record.Currency, record.Value, record.Description));
                }
            }
            
            // "remove" records after this day from balance
            foreach (var record in _preloadedData.Records.Where(item => date.AddDays(1) < item.Date))
            {
                data.Balance += ConvertToMainCurrency(record.TransactionType == TransactionType.Expense ? record.Value : -record.Value, record.CurrencyId);
            }
        }

        private void FillMoneyTransfersLegend(CalendarDayData data, DateTime date, List<int> storageGroupIds)
        {
            // write legend for today money transfers
            foreach (var moneyTransfer in _preloadedData.MoneyTransfers.Where(item
                => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth).ToList())
            {
                var currencyFrom = _preloadedData.Currencies.First(_ => _.Id == moneyTransfer.StorageFrom.CurrencyId).ToReferenceView();
                var currencyTo = _preloadedData.Currencies.First(_ => _.Id == moneyTransfer.StorageTo.CurrencyId).ToReferenceView();
                
                data.MoneyTransferLegend.Add(new MoneyTransferLegendUnit
                {
                    StorageFrom = moneyTransfer.StorageFrom,
                    StorageTo = moneyTransfer.StorageTo,
                    ValueCurrencyFrom = new ValueUnit(currencyFrom, moneyTransfer.Value),
                    ValueCurrencyTo = new ValueUnit(currencyTo, moneyTransfer.Value * moneyTransfer.CurrencyExchangeRate)
                });
            }
            
            // get all money transfers after this day between storages with differens currencies
            var moneyTransfers = _preloadedData.MoneyTransfers.Where(item => item.Date >= date.AddDays(1)
                && item.StorageFrom.CurrencyId != item.StorageTo.CurrencyId).ToList();

            // revert all money transfers after this day between storages with differens currencies
            foreach (var moneyTransfer in moneyTransfers)
            {
                // TODO: not calculate the same for every day
                if (storageGroupIds.Contains(moneyTransfer.StorageFrom.StorageGroupId))
                    data.Balance += ConvertToMainCurrency(moneyTransfer.Value, moneyTransfer.StorageFrom.CurrencyId);
                if (storageGroupIds.Contains(moneyTransfer.StorageTo.StorageGroupId))
                    data.Balance += ConvertToMainCurrency(-moneyTransfer.Value * moneyTransfer.CurrencyExchangeRate, moneyTransfer.StorageTo.CurrencyId);
            }
        }

        private void FillEventLegend(CalendarDayData data, DateTime date, List<int> storageGroupIds)
        {
            // TODO: remove previous events from balance
            // TODO: write events in order
            // write every month events
            foreach (var schedule in _preloadedData.Schedules.MonthlySchedules.Where(e => e.DayOfMonth == date.Day
                && e.Months.Contains((Month)date.Month)).Distinct())
            {
                // write event
                WriteEvent(data, schedule.EventId, storageGroupIds);
            }

            // write every week events
            foreach (var schedule in _preloadedData.Schedules.WeeklySchedules.Where(item => item.DaysOfWeek.Contains(date.DayOfWeek)))
            {
                // write event
                WriteEvent(data, schedule.EventId, storageGroupIds);
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
                WriteEvent(data, schedule.EventId, storageGroupIds);
            }

            // write once events
            foreach (var schedule in _preloadedData.Schedules.OnceSchedules.Where(item => item.Date.Year == data.Year && item.Date.Month == data.Month && item.Date.Day == data.DayOfMonth))
            {
                // write event
                WriteEvent(data, schedule.EventId, storageGroupIds);
            }
        }

        private void WriteEvent(CalendarDayData data, int eventId, List<int> storageGroupIds)
        {
            // get event
            var eventType = _preloadedData.Events.GetEventType(eventId);

            if(eventType == EventType.Simple)
            {
                var evnt = _preloadedData.Events.SimpleEvents.First(_ => _.Id == eventId);
                if (!storageGroupIds.Contains(evnt.Storage.StorageGroupId)) return;

                // update balance
                var valueInMainCurrency = ConvertToMainCurrency(evnt.Value, evnt.CurrencyId);
                if (evnt.TransactionType == TransactionType.Expense)
                {
                    data.TotalExpences += valueInMainCurrency;
                    data.Balance -= valueInMainCurrency;
                }
                else
                {
                    data.TotalIncomes += valueInMainCurrency;
                    data.Balance += valueInMainCurrency;
                }

                // fill legend
                data.Legend.Add(new LegendUnit(evnt.Currency, evnt.TransactionType == TransactionType.Expense ? -evnt.Value : evnt.Value, evnt.Description));
            }

            if(eventType == EventType.RepayDebt)
            {
                var evnt = _preloadedData.Events.RepayDebtEvents.First(_ => _.Id == eventId);
                if (!storageGroupIds.Contains(evnt.Storage.StorageGroupId)) return;

                // TODO: curency? from storage or from debt
                // update balance
                var valueInMainCurrency = ConvertToMainCurrency(evnt.Value, evnt.Storage.CurrencyId);
                if (evnt.Debt.DebtType == DebtType.GiveBorrow)
                {
                    data.TotalExpences += valueInMainCurrency;
                    data.Balance -= valueInMainCurrency;
                }
                else
                {
                    data.TotalIncomes += valueInMainCurrency;
                    data.Balance += valueInMainCurrency;
                }

                // fill legend
                var currency = _preloadedData.Currencies.First(_ => _.Id == evnt.Storage.CurrencyId).ToReferenceView();
                data.Legend.Add(new LegendUnit(currency, evnt.Debt.DebtType == DebtType.GiveBorrow ? -evnt.Value : evnt.Value, evnt.Description));
            }

            if(eventType == EventType.MoneyTransfer)
            {
                var evnt = _preloadedData.Events.MoneyTransferEvents.First(_ => _.Id == eventId);
                if (!storageGroupIds.Contains(evnt.StorageFrom.StorageGroupId) && !storageGroupIds.Contains(evnt.StorageTo.StorageGroupId)) return;

                // update balance
                if (!(storageGroupIds.Contains(evnt.StorageFrom.StorageGroupId) && storageGroupIds.Contains(evnt.StorageTo.StorageGroupId)))
                {
                    if (storageGroupIds.Contains(evnt.StorageFrom.StorageGroupId))
                    {
                        var valueInMainCurrency = ConvertToMainCurrency(evnt.Value, evnt.StorageFrom.CurrencyId);
                        data.TotalExpences += valueInMainCurrency;
                        data.Balance -= valueInMainCurrency;
                    }
                    if (storageGroupIds.Contains(evnt.StorageTo.StorageGroupId))
                    {
                        var valueInMainCurrency = ConvertToMainCurrency(evnt.Value, evnt.StorageTo.CurrencyId);
                        data.TotalIncomes += valueInMainCurrency;
                        data.Balance += valueInMainCurrency;
                    }
                }
                   
                var currencyFrom = _preloadedData.Currencies.First(_ => _.Id == evnt.StorageFrom.CurrencyId).ToReferenceView();
                var currencyTo = _preloadedData.Currencies.First(_ => _.Id == evnt.StorageTo.CurrencyId).ToReferenceView();

                // TODO: load from database evnt.StorageTo.CurrencyId currency exchange rate
                var valCurrTo = evnt.TakeExistingCurrencyExchangeRate
                    ? CalculationHelper.ConvertToCurrency(evnt.Value, evnt.StorageFrom.CurrencyId, evnt.StorageTo.CurrencyId, _currencyExchangeRates)
                    : evnt.Value * evnt.CurrencyExchangeRate;

                // fill legend
                data.MoneyTransferLegend.Add(new MoneyTransferLegendUnit()
                {
                    StorageFrom = evnt.StorageFrom,
                    StorageTo = evnt.StorageTo,
                    ValueCurrencyFrom = new ValueUnit(currencyFrom, evnt.Value),
                    ValueCurrencyTo = new ValueUnit(currencyTo, valCurrTo)
                });

                // TODO: fill comission
                if(evnt.Commission > 0)
                {
                    // calculate comission value correspond to comission tyep
                    var comissionValue = evnt.CommissionType == CommissionType.Currency ? evnt.Commission : evnt.Value * evnt.Commission;
                    //var comissionCurrency = evnt.
                    // update balance
                    if (evnt.TakeComissionFromReceiver && storageGroupIds.Contains(evnt.StorageTo.StorageGroupId))
                    {
                        var valueInMainCurrency = ConvertToMainCurrency(comissionValue, evnt.StorageTo.CurrencyId);
                        data.TotalExpences += valueInMainCurrency;
                        data.Balance += valueInMainCurrency;
                    }
                    // ???
                    if (storageGroupIds.Contains(evnt.StorageFrom.StorageGroupId))
                    {
                        var valueInMainCurrency = ConvertToMainCurrency(evnt.Value, evnt.StorageFrom.CurrencyId);
                        data.TotalExpences += valueInMainCurrency;
                        data.Balance -= valueInMainCurrency;
                    }
                    if (storageGroupIds.Contains(evnt.StorageTo.StorageGroupId))
                    {
                        var valueInMainCurrency = ConvertToMainCurrency(evnt.Value, evnt.StorageTo.CurrencyId);
                        data.TotalIncomes += valueInMainCurrency;
                        data.Balance += valueInMainCurrency;
                    }
                }
            }
        }

        private decimal ConvertToMainCurrency(decimal value, int currencyFromId)
        {
            if (currencyFromId == _mainCurrency.Id) return value;
            else return CalculationHelper.ConvertToCurrency(value, currencyFromId, _mainCurrency.Id, _currencyExchangeRates);
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
