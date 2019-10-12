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
        private ILimitService _limitService;
        private ICategoryService _categoryService;
        private CalendarData _data;

        #endregion

        #region Initialization

        public CalendarDataBuilder(int userId,
            ITransactionService transactionService,
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService,
            IStorageService storageService,
            ILimitService limitService,
            ICategoryService categoryService)
        {
            _userId = userId;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
            _storageService = storageService;
            _limitService = limitService;
            _categoryService = categoryService;
            _data = new CalendarData();
        }

        #endregion

        #region Public methods

        public List<CalendarDayData> Build(DateTime dateFrom, DateTime dateUntil)
        {
            // prepare
            LoadCalendarData();

            // load data (transactions and limits) for selected period
            var transactions = _transactionService.Get(_userId, dateFrom, dateUntil);
            var limits = _limitService.Get(_userId, dateFrom, dateUntil);
            // result data list
            var result = new List<CalendarDayData>();

            // loop for every day
            DateTime currDate = dateFrom.Date;
            while (currDate <= dateUntil)
            {
                // create calendar day data
                result.Add(CreateCalendarDayData(currDate, transactions, limits));
                // next day
                currDate = currDate.AddDays(1);
            }

            // load transactions that include Today if selection doesn't include it
            var missingTransactions = dateFrom > DateTime.Today 
                ? _transactionService.Get(_userId, DateTime.Today, dateFrom.AddDays(-1))
                : (dateUntil < DateTime.Today 
                    ? _transactionService.Get(_userId, dateUntil.Date.AddDays(1), DateTime.Today.AddDays(1).AddMilliseconds(-1)) 
                    : new List<ITransaction>());

            // update storages and limits state
            UpdateRelatedItemsState(result, missingTransactions);

            return result;
        }

        #endregion

        #region Private methods

        private CalendarDayData CreateCalendarDayData(DateTime date, List<ITransaction> transactions, List<LimitModel> limits)
        {
            // create calendar day data object
            var calendarDayData = new CalendarDayData(date) { CalendarData = _data };

            // fill existing records and money transfers in this day
            calendarDayData.Transactions = transactions.Where(x => x.TransactionDate.Day == calendarDayData.DayOfMonth
                && x.TransactionDate.Month == calendarDayData.Month && x.TransactionDate.Year == calendarDayData.Year).ToList();
            calendarDayData.FilteredTransactions = calendarDayData.Transactions;

            // populate initial limit state in this day
            calendarDayData.Limits = limits.Where(x => x.DateFrom <= calendarDayData.Date && calendarDayData.Date <= x.DateUntil)
                .Select(x => new Model.Calendar.LimitState()
                {
                    Limit = x,
                    SpentValue = x.SpentValue,
                    Categories = _data.Categories.Where(e => x.CategoryIds.Contains(e.Id)).ToList()
                }).ToList();
            calendarDayData.FilteredLimits = calendarDayData.Limits;

            // populate storage initial state
            calendarDayData.Storages = _data.Storages.Select(x => new StorageState()
            {
                Storage = x,
                Amount = x.Value
            }).ToList();
            calendarDayData.FilteredStorages = calendarDayData.Storages;

            return calendarDayData;
        }

        private void UpdateRelatedItemsState(List<CalendarDayData> calendarDays, List<ITransaction> missingTransactions)
        {
            // cancatenate all transactions for updating storages/limits state
            var alltransactions = calendarDays.SelectMany(x => x.Transactions).Union(missingTransactions).ToList();

            // update past days
            foreach (var calendarDay in calendarDays.Where(x => !x.IsFutureDay && !x.IsToday).ToList())
            {
                // get actual transactions after current date
                var transactions = alltransactions
                    .Where(x => !x.IsPlanned && x.TransactionDate > calendarDay.Date.AddDays(1).AddMilliseconds(-1))
                    .ToList();

                // update storages state
                UpdatePastDateStorageState(calendarDay, transactions);
                // update limits state
                UpdateLimitState(calendarDay, transactions, true);
            }

            // update future days
            foreach (var calendarDay in calendarDays.Where(x => x.IsFutureDay).ToList())
            {
                // get planned transactions before current date
                var transactions = alltransactions
                    .Where(x => x.IsPlanned && x.TransactionDate <= calendarDay.Date.AddDays(1).AddMilliseconds(-1))
                    .ToList();

                // update storages state
                UpdateFutureDateStorageState(calendarDay, transactions);
                // update limits state
                UpdateLimitState(calendarDay, transactions, false);
            }
        }

        private void UpdatePastDateStorageState(CalendarDayData calendarDay, List<ITransaction> transactions)
        {
            foreach (var storageState in calendarDay.Storages)
            {
                // update storage state by records
                storageState.Amount -= transactions
                    .Where(x => (x.TransactionType == TransactionType.Expense || x.TransactionType == TransactionType.Income)
                        && x.TransactionStorage.Id == storageState.Storage.Id).Sum(x => x.TransactionAmount);

                // get related money transfers
                var moneyTransfers = transactions
                    .Where(x => x.TransactionStorageIds.Contains(storageState.Storage.Id) 
                        && x.TransactionType == TransactionType.MoneyTransfer)
                    .Select(x => x as MoneyTransferModel)
                    .ToList();

                // revert money transfers
                storageState.Amount += moneyTransfers.Where(x => x.StorageFromId == storageState.Storage.Id).Sum(x => x.StorageFromValue);
                storageState.Amount -= moneyTransfers.Where(x => x.StorageToId == storageState.Storage.Id).Sum(x => x.StorageToValue);
            }
        }

        private void UpdateFutureDateStorageState(CalendarDayData calendarDay, List<ITransaction> transactions)
        {
            foreach (var storageState in calendarDay.Storages)
            {
                // update storage state by records
                storageState.Amount += transactions
                    .Where(x => (x.TransactionType == TransactionType.Expense || x.TransactionType == TransactionType.Income)
                        && x.TransactionStorage.Id == storageState.Storage.Id).Sum(x => x.TransactionAmount);

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

        private void UpdateLimitState(CalendarDayData calendarDay, List<ITransaction> transactions, bool isPast)
        {
            // for past days remove from spent value, for future - add
            var sign = isPast ? -1 : 1;

            foreach (var limitState in calendarDay.Limits)
            {
                // update limit state (Note: Transaction amount is negative for expense)
                limitState.SpentValue += sign * (- transactions
                    .Where(x => limitState.Limit.DateFrom <= x.TransactionDate && x.TransactionDate <= limitState.Limit.DateUntil
                        && (limitState.Limit.CategoryIds.Count == 0 || limitState.Limit.CategoryIds.Contains(x.TransactionCategory?.Id ?? -1))
                        && x.IsExpense)
                    .Sum(x => x.TransactionAmount));
            }
        }

        private void LoadCalendarData()
        {
            // TODO: reload only if main currency or currency exchange rate was changed
            _data.MainCurrency = _currencyService.GetMain(_userId).ToReferenceView();
            _data.Rates = _currencyExchangeRateService.GetList(_userId, _data.MainCurrency.Id);

            // always reload
            _data.Storages = _storageService.GetListForUser(_userId);
            _data.Categories = _categoryService.GetListForUser(_userId);
        }

        #endregion
    }
}
