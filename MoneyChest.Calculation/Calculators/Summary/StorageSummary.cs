using MoneyChest.Calculation.Common;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Summary
{
    /// <summary>
    /// Key - storage group id
    /// </summary>
    public class StorageSummary : Dictionary<StorageGroupReference, ValueUnitCollection>
    {
        internal void Update(StorageGroupReference storageGroup, CurrencyReference currency, decimal value)
        {
            if (!this.Keys.Any(e => e.Id == storageGroup.Id))
            {
                var collection = new ValueUnitCollection();
                collection.Add(new ValueUnit(currency, value));
                this.Add(storageGroup, collection);
            }
            else
            {
                var key = this.Keys.First(e => e.Id == storageGroup.Id);
                this[key].Update(currency, value);
            }
        }
    }
}
