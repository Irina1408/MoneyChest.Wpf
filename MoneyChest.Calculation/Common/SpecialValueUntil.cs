using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;

namespace MoneyChest.Calculation.Common
{
    public class SpecialValueUntil<T> : ValueUnit
    {
        public SpecialValueUntil(int currencyId, T special) : base(currencyId)
        {
            Special = special;
        }

        public T Special { get; }
    }
}
