using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Report
{
    public class ReportUnit
    {
        public ReportUnit(string caption, decimal amount)
        {
            Caption = caption;
            Amount = amount;
        }
        
        public decimal Amount { get; set; }
        public string Caption { get; }
    }
}
