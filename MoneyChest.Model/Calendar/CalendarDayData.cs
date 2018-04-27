using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Calendar
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class CalendarDayData
    {
        #region Initialization

        public CalendarDayData(DateTime date)
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
        public bool IsFutureDay => Date > DateTime.Today;

        #endregion

        #region Legend

        public List<ITransaction> Transactions { get; set; } = new List<ITransaction>();
        public List<StorageState> Storages { get; set; } = new List<StorageState>();

        #endregion

        #region Filtered legend

        public List<ITransaction> FilteredTransactions { get; set; }
        public List<StorageState> FilteredStorages { get; set; }

        #endregion

        #region Totals & Summaries

        // TODO: filter by storage
        public decimal TotDayAmount => FilteredTransactions.Sum(x => ToMainCurrency(x.TransactionAmount, x.TransactionCurrencyId));
        public decimal TotStorageSummary => FilteredStorages.Sum(x => ToMainCurrency(x.Amount, x.Storage.CurrencyId));

        public string TotDayAmountDetailed => FormatMainCurrency(TotDayAmount, true);
        public string TotStorageSummaryDetailed => FormatMainCurrency(TotStorageSummary, false, false);

        #endregion

        #region Helper references

        public CalendarData CalendarData { get; set; }

        #endregion

        #region Private methods

        private decimal ToMainCurrency(decimal val, int currencyId) => 
            val != 0 ? ConvertToCurrency(val, currencyId, CalendarData.MainCurrency.Id, CalendarData.Rates) : 0;

        private string FormatMainCurrency(decimal val, bool hideZero = false, bool showSign = true) =>
            hideZero && val == 0 ? null : CalendarData.MainCurrency.FormatValue(val, showSign);

        /// <summary>
        /// TODO: remove from here. MoneyChest.Calculation.Common.ConvertToCurrency
        /// </summary>
        private decimal ConvertToCurrency(decimal value, int currencyId, int targetCurrencyId,
            IEnumerable<CurrencyExchangeRateModel> rates)
        {
            if (currencyId == targetCurrencyId) return value;
            var rate = rates.FirstOrDefault(item => item.CurrencyFromId == currencyId && item.CurrencyToId == targetCurrencyId);
            return value * (rate != null ? rate.Rate : 1);
        }

        #endregion
    }
}
