using MoneyChest.Calculation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Summary.Base
{
    public class BaseSummary<T>
    {
        public BaseSummary()
        {
            Items = new List<SpecialValueUntil<T>>();
        }

        public List<SpecialValueUntil<T>> Items { get; }

        internal void UpdateBalance(T special, ValueUnit valueUnit)
        {
            UpdateBalance(special, valueUnit.CurrencyId, valueUnit.Value);
        }

        internal void UpdateBalance(T special, int currencyId, decimal value)
        {
            var bal = Items.FirstOrDefault(_ => _.Special.Equals(special) && _.CurrencyId == currencyId);
            if (bal == null)
                Items.Add(new SpecialValueUntil<T>(currencyId, special) { Value = value });
            else
                bal.Value += value;
        }
    }
}
