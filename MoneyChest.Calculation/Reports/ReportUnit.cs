using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Reports
{
    public class ReportUnit
    {
        public ReportUnit(string caption)
        {
            Caption = caption;
        }

        public ReportUnit(string caption, decimal value)
        {
            Caption = caption;
            Value = value;
        }

        public string Caption { get; private set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
    }
}
