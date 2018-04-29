using MoneyChest.Model.Calendar;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders
{
    public class CalendarDataBuilder
    {
        #region Private fields

        private int _userId;
        private ITransactionService _transactionService;
        private ICurrencyService _currencyService;
        private ICurrencyExchangeRateService _currencyExchangeRateService;
        private IStorageService _storageService;
        private CalendarData _data;

        #endregion

        #region Initialization

        public CalendarDataBuilder(int userId,
            ITransactionService transactionService,
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService,
            IStorageService storageService)
        {
            _userId = userId;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _storageService = storageService;
            _data = new CalendarData();
        }

        #endregion

        #region Public methods

        public List<CalendarDayData> Build(DateTime dateFrom, DateTime dateUntil)
        {
            // prepare
            LoadCalendarData();

            // load transactions for selected period
            var transactions = _transactionService.Get(_userId, dateFrom, dateUntil);
            // result data list
            var result = new List<CalendarDayData>();

            // loop for every day
            DateTime currDate = dateFrom.Date;
            while (currDate <= dateUntil)
            {
                // create calendar day data object
                var calendarDayData = new CalendarDayData(currDate) { CalendarData = _data };

                // fill existing records and money transfers in this day
                calendarDayData.Transactions = transactions.Where(x => x.TransactionDate.Day == calendarDayData.DayOfMonth
                    && x.TransactionDate.Month == calendarDayData.Month && x.TransactionDate.Year == calendarDayData.Year).ToList();
                calendarDayData.FilteredTransactions = calendarDayData.Transactions;
                // populate storage initial state
                calendarDayData.Storages = _data.Storages.Select(x => new StorageState()
                {
                    Storage = x,
                    Amount = x.Value
                }).ToList();
                calendarDayData.FilteredStorages = calendarDayData.Storages;

                result.Add(calendarDayData);
                currDate = currDate.AddDays(1);
            }

            // load transactions that include Today if selection doesn't include it
            var missingTransactions = dateFrom > DateTime.Today 
                ? _transactionService.Get(_userId, DateTime.Today, dateFrom.AddDays(-1))
                : (dateUntil < DateTime.Today 
                    ? _transactionService.Get(_userId, dateUntil.Date.AddDays(1), DateTime.Today.AddDays(1).AddMilliseconds(-1)) 
                    : new List<ITransaction>());

            // update storages state
            UpdateStorageState(result, missingTransactions);

            return result;
        }

        #endregion

        #region Private methods

        private void UpdateStorageState(List<CalendarDayData> calendarDays, List<ITransaction> missingTransactions)
        {
            // update past days
            var alltransactions = calendarDays.SelectMany(x => x.Transactions).Union(missingTransactions).ToList();

            foreach (var calendarDay in calendarDays.Where(x => !x.IsFutureDay && !x.IsToday).ToList())
            {
                foreach (var storageState in calendarDay.Storages)
                {
                    // get actual transactions after current date
                    var transactions = alltransactions
                        .Where(x => !x.IsPlanned && x.TransactionDate > calendarDay.Date.AddDays(1).AddMilliseconds(-1) 
                            && x.TransactionStorageIds.Contains(storageState.Storage.Id))
                        .ToList();
                    
                    storageState.Amount -= transactions
                        .Where(x => x.TransactionStorage.Id == storageState.Storage.Id).Sum(x => x.TransactionAmount);

                    // get related money transfers
                    var moneyTransfers = transactions
                        .Where(x => x.TransactionStorageIds.Contains(storageState.Storage.Id) 
                            && x.TransactionType ==  TransactionType.MoneyTransfer)
                        .Select(x => x as MoneyTransferModel)
                        .ToList();

                    // revert money transfers
                    storageState.Amount += moneyTransfers.Where(x => x.StorageFromId == storageState.Storage.Id).Sum(x => x.StorageFromValue);
                    storageState.Amount -= moneyTransfers.Where(x => x.StorageToId == storageState.Storage.Id).Sum(x => x.StorageToValue);
                }
            }

            // update future days
            foreach (var calendarDay in calendarDays.Where(x => x.IsFutureDay).ToList())
            {
                foreach(var storageState in calendarDay.Storages)
                {
                    // get planned transactions before current date
                    var transactions = alltransactions
                        .Where(x => x.IsPlanned && x.TransactionDate <= calendarDay.Date.AddDays(1).AddMilliseconds(-1)
                            && x.TransactionStorageIds.Contains(storageState.Storage.Id))
                        .ToList();

                    storageState.Amount += transactions
                        .Where(x => x.TransactionStorage.Id == storageState.Storage.Id).Sum(x => x.TransactionAmount);

                    // get related money transfers
                    var moneyTransfers = transactions
                        .Where(x => x.TransactionStorageIds.Contains(storageState.Storage.Id) 
                            && x.TransactionType == TransactionType.MoneyTransfer)
                        .Select(x => (x as PlannedTransactionModel<EventModel>).Event as MoneyTransferEventModel)
                        .ToList();

                    // apply money transfers
                    storageState.Amount -= moneyTransfers.Where(x => x.StorageFromId == storageState.Storage.Id).Sum(x => x.StorageFromValue);
                    storageState.Amount += moneyTransfers.Where(x => x.StorageToId == storageState.Storage.Id).Sum(x => x.StorageToValue);
                }
            }
        }

        private void LoadCalendarData()
        {
            // TODO: reload only if main currency or currency exchange rate was changed
            _data.MainCurrency = _currencyService.GetMain(_userId).ToReferenceView();
            _data.Rates = _currencyExchangeRateService.GetList(_userId, _data.MainCurrency.Id);

            // always reload
            _data.Storages = _storageService.GetListForUser(_userId);
        }

        #endregion
    }
}
