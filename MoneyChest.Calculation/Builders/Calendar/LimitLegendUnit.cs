using MoneyChest.Calculation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class LimitLegendUnit : LegendUnit
    {
        public int LimitId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public decimal RemainingValue { get; set; }
        public bool IsExceeded { get; set; }
    }
}
