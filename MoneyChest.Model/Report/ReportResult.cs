using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Report
{
    public class ReportResult
    {
        #region Transactions

        public List<ITransaction> Transactions { get; set; } = new List<ITransaction>();
        public List<ITransaction> FilteredTransactions { get; set; }

        #endregion

        #region Helper references

        public ReportData ReportData { get; set; }

        #endregion

        #region Result

        public List<ReportUnit> ReportUnits { get; set; }

        #endregion

        #region Totals

        public decimal TotAmount => ReportUnits.Sum(x => x.Amount);
        public string TotAmountDetailed => FormatMainCurrency(TotAmount, true);

        #endregion

        #region Private methods

        private string FormatMainCurrency(decimal val, bool hideZero = false, bool showSign = false) =>
            hideZero && val == 0 ? null : ReportData.MainCurrency.FormatValue(val, showSign);

        #endregion
    }
}
