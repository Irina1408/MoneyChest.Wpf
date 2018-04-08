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
        private CalendarData _data;

        #endregion

        #region Initialization

        public CalendarDataBuilder(int userId,
            ITransactionService transactionService,
            ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService)
        {
            _userId = userId;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _currencyExchangeRateService = currencyExchangeRateService;
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
                var calendarDayData = new CalendarDayData(currDate)
                {
                    CalendarData = _data
                };

                // fill existing records and money transfers in this day
                calendarDayData.Transactions = transactions.Where(x => x.TransactionDate.Day == calendarDayData.DayOfMonth
                    && x.TransactionDate.Month == calendarDayData.Month && x.TransactionDate.Year == calendarDayData.Year).ToList();

                result.Add(calendarDayData);
                currDate = currDate.AddDays(1);
            }

            return result;
        }

        #endregion

        #region Private methods

        private void LoadCalendarData()
        {
            // TODO: reload only if main currency or currency exchange rate was changed
            _data.MainCurrency = _currencyService.GetMain(_userId).ToReferenceView();
            _data.Rates = _currencyExchangeRateService.GetList(_userId, _data.MainCurrency.Id);
        }

        #endregion
    }
}
