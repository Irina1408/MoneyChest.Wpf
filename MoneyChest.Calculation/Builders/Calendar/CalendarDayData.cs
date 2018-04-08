using MoneyChest.Calculation.Common;
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

        public decimal TotDayAmount => FilteredTransactions
            // TODO: filter by storage
            .Sum(x => ToMainCurrency(x.TransactionAmount, x.TransactionCurrencyId));

        public string TotDayAmountDetailed => FormatMainCurrency(TotDayAmount, true);

        #endregion

        #region Summaries



        #endregion

        #region Helper references

        internal CalendarData CalendarData { get; set; }

        #endregion

        #region Private methods

        private decimal ToMainCurrency(decimal val, int currencyId) => 
            val != 0 ? CalculationHelper.ConvertToCurrency(val, currencyId, CalendarData.MainCurrency.Id, CalendarData.Rates) : 0;

        private string FormatMainCurrency(decimal val, bool hideZero = false) =>
            hideZero && val == 0 ? null : CalendarData.MainCurrency.FormatValue(val, true);

        #endregion
    }
}
