using MoneyChest.Calculation.Common;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class MoneyTransferLegendUnit //: LegendUnit
    {
        public string Description { get; set; }

        public StorageReference StorageFrom { get; set; }
        public StorageReference StorageTo { get; set; }
        public ValueUnit ValueCurrencyFrom { get; set; }
        // TODO: include CurrencyExchangeRate
        public ValueUnit ValueCurrencyTo { get; set; }
        public ValueUnit Comission { get; set; }
        public bool TakeComissionFromReceiver { get; set; }

        public CategoryReference Category { get; set; }
        public bool IsPlanned { get; set; } = false;
    }
}
