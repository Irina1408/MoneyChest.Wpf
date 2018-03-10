using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TransactionsViewSettingsModel : IHasUserId
    {
        public TransactionsViewSettingsModel()
        {
            PeriodType = PeriodType.Month;
        }

        public int UserId { get; set; }
        public PeriodType PeriodType { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateUntil { get; set; }


    }
}
