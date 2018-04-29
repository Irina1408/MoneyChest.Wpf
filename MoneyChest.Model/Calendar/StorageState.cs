using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Calendar
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class StorageState
    {
        public StorageModel Storage { get; set; }
        public decimal Amount { get; set; }
        public string AmountDetailed => Storage.Currency?.FormatValue(Amount) ?? Amount.ToString();
        public bool IsNegative => Amount < 0;
    }
}
