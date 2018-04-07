using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders
{
    public class CalendarDayData
    {
        #region Initialization

        public CalendarDayData(DateTime date)
            : base()
        {
            Date = date.Date;
        }

        #endregion

        #region Date definition

        public DateTime Date { get; private set; }
        public int DayOfMonth => Date.Day;
        public int Month => Date.Month;
        public int Year => Date.Year;
        public bool IsToday => Date == DateTime.Today;
        public bool IsWeekend => Date.DayOfWeek == DayOfWeek.Sunday || Date.DayOfWeek == DayOfWeek.Saturday;

        #endregion

        #region Legend

        public List<ITransaction> Transactions { get; set; } = new List<ITransaction>();

        #endregion

        #region Filtered legend

        public List<ITransaction> FilteredTransactions => Transactions;

        #endregion

        #region Totals



        #endregion

        #region Summaries



        #endregion
    }
}
