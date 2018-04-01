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

        #endregion

        #region Initialization

        public CalendarDataBuilder(int userId,
            ITransactionService transactionService)
        {
            _userId = userId;
            _transactionService = transactionService;
        }

        #endregion

        #region Public methods

        public List<CalendarDayData> Build(DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<CalendarDayData>();

            // load transactions for selected period
            var transactions = _transactionService.Get(_userId, dateFrom, dateUntil);

            // loop for every day
            DateTime currDate = dateFrom.Date;
            while (currDate <= dateUntil)
            {
                // create calendar day data object
                var calendarDayData = new CalendarDayData(currDate);
                // fill existing records and money transfers in this day
                calendarDayData.Transactions = transactions.Where(x => x.TransactionDate.Day == calendarDayData.DayOfMonth
                    && x.TransactionDate.Month == calendarDayData.Month && x.TransactionDate.Year == calendarDayData.Year).ToList();

                result.Add(calendarDayData);
                currDate = currDate.AddDays(1);
            }

            return result;
        }

        #endregion
    }
}
