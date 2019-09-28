using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Report
{
    public class ReportBuildSettings
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public int CategoryLevel { get; set; }
        public RecordType DataType { get; set; }
        public Sorting Sorting { get; set; }
        public int DetailsDepth { get; set; }
        public BarChartSection Section { get; set; }
        public PeriodType PeriodType { get; set; }
        public bool IncludeActualTransactions { get; set; }
        public bool IncludeFuturePlannedTransactions { get; set; }

        public Func<IEnumerable<ITransaction>, List<ITransaction>> ApplyFilter { get; set; }
    }
}
