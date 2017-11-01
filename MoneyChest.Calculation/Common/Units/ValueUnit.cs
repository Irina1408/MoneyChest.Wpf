using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    /// <summary>
    /// Value of some currency
    /// </summary>
    public class ValueUnit
    {
        public ValueUnit()
        {
        }

        public ValueUnit(CurrencyReference currency)
        {
            Currency = currency;
        }

        public ValueUnit(CurrencyReference currency, decimal value)
            : this(currency)
        {
            Value = value;
        }

        public decimal Value { get; set; }
        public CurrencyReference Currency { get; set; }
    }
}
