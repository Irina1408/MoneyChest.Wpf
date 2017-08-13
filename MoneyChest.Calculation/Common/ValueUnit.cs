using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    public class ValueUnit
    {
        public ValueUnit(int currencyId)
        {
            CurrencyId = currencyId;
        }

        public int CurrencyId { get; }
        public decimal Value { get; set; }
    }
}
