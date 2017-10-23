using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    public class LegendUnit : ValueUnit
    {
        public LegendUnit()
        { }

        public LegendUnit(CurrencyReference currency) : base(currency)
        { }

        public LegendUnit(CurrencyReference currency, decimal value) : base(currency, value)
        { }

        public LegendUnit(CurrencyReference currency, decimal value, string description) : base(currency, value)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
