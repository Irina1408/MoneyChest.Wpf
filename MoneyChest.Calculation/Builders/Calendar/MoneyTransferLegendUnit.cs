using MoneyChest.Calculation.Common;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class MoneyTransferLegendUnit
    {
        public StorageReference StorageFrom { get; set; }
        public StorageReference StorageTo { get; set; }
        public ValueUnit ValueCurrencyFrom { get; set; }
        public ValueUnit ValueCurrencyTo { get; set; }

        public string Description => $"{StorageFrom.Name} -> {StorageTo.Name}";
    }
}
