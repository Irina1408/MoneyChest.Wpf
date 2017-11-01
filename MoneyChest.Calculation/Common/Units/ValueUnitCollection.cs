using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    public class ValueUnitCollection : List<ValueUnit>
    {
        internal void Update(CurrencyModel currency, decimal value)
        {
            var bal = this.FirstOrDefault(_ => _.Currency.Id == currency.Id);
            if (bal == null)
                this.Add(new ValueUnit(currency.ToReferenceView(), value));
            else
                bal.Value += value;
        }

        internal void Update(CurrencyReference currency, decimal value)
        {
            var bal = this.FirstOrDefault(_ => _.Currency.Id == currency.Id);
            if (bal == null)
                this.Add(new ValueUnit(currency, value));
            else
                bal.Value += value;
        }
    }
}
