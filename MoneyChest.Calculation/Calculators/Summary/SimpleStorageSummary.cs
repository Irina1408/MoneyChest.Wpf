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
    public class SimpleStorageSummaryUnit : ValueUnit
    {
        public SimpleStorageSummaryUnit(CurrencyReference currency) : base(currency)
        { }

        public SimpleStorageSummaryUnit(CurrencyReference currency, decimal value) : base(currency, value)
        { }

        public StorageGroupReference StorageGroup { get; set; }
    }

    public class SimpleStorageSummary : List<SimpleStorageSummaryUnit>
    {
        internal void Update(StorageGroupReference storageGroup, CurrencyReference currency, decimal value)
        {
            if (!this.Any(e => e.StorageGroup.Id == storageGroup.Id && e.Currency.Id == currency.Id))
            {
                this.Add(new SimpleStorageSummaryUnit(currency)
                {
                    StorageGroup = storageGroup,
                    Value = value
                });
            }
            else
            {
                var summaryItem = this.First(e => e.StorageGroup.Id == storageGroup.Id && e.Currency.Id == currency.Id);
                summaryItem.Value += value;
            }
        }
    }
}
