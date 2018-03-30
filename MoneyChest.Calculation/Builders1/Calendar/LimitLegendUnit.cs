using MoneyChest.Calculation.Common;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class LimitLegendUnit //: LegendUnit
    {
        internal int LimitId { get; set; }

        public string Description { get; set; }
        public decimal Value { get; set; }
        public CurrencyReference Currency { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public decimal RemainingValue { get; set; }
        public bool IsExceeded { get; set; }
    }
}
