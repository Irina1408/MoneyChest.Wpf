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
        public List<ReportUnit> ReportUnits { get; set; }
        public Dictionary<int, List<ReportUnit>> Detailing { get; set; } = new Dictionary<int, List<ReportUnit>>();
        public string TotAmountDetailed { get; set; }

        //public decimal TotAmount => ReportUnits.Sum(x => x.Amount);
        //public string TotAmountDetailed => FormatMainCurrency(TotAmount, true);

        //#region Helper references

        //public ReportData ReportData { get; set; }

        //#endregion

        //#region Private methods

        //private string FormatMainCurrency(decimal val, bool hideZero = false, bool showSign = false) =>
        //    hideZero && val == 0 ? null : ReportData.MainCurrency.FormatValue(val, showSign);

        //#endregion
    }
}
