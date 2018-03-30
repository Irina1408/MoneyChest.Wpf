using MoneyChest.Calculation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Reports
{
    public class ReportUnit
    {
        public ReportUnit(string caption, decimal value)
        {
            Caption = caption;
            Value = value;
        }

        /// <summary>
        /// Percentage of spend/received value from total expenses/incomes
        /// </summary>
        public decimal Percentage { get; set; }
        public decimal Value { get; set; }
        public string Caption { get; }
    }
}
