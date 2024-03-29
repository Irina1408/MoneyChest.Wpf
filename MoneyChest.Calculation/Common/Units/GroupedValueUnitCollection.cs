﻿using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    public class GroupedValueUnitCollection<TKey> : Dictionary<TKey, ValueUnitCollection>
    {
        internal virtual void Update(TKey key, CurrencyModel currency, decimal value)
        {
            if (!this.ContainsKey(key))
            {
                var collection = new ValueUnitCollection();
                collection.Add(new ValueUnit(currency.ToReferenceView(), value));
                this.Add(key, collection);
            }
            else
            {
                this[key].Update(currency, value);
            }
        }

        internal virtual void Update(TKey key, CurrencyReference currency, decimal value)
        {
            if(!this.ContainsKey(key))
            {
                var collection = new ValueUnitCollection();
                collection.Add(new ValueUnit(currency, value));
                this.Add(key, collection);
            }
            else
            {
                this[key].Update(currency, value);
            }
        }
    }
}
