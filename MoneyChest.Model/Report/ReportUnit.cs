using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Report
{
    public class ReportUnit
    {
        public ReportUnit()
        { }

        public ReportUnit(int? categoryId, string caption, decimal amount)
        {
            CategoryId = categoryId;
            Caption = caption;
            Amount = amount;
        }
        
        public int? CategoryId { get; set; }
        public string Caption { get; set; }
        public decimal Amount { get; set; }
        public double DoubleAmount => Math.Round(Convert.ToDouble(Amount), 2);
        public ReportUnitColor Color { get; set; } = ReportUnitColor.Any;

        public List<ReportUnit> Detailing { get; set; } = new List<ReportUnit>();
    }

    public enum ReportUnitColor
    {
        Any = 0,
        Red = 1,
        Green = 2
    }
}
