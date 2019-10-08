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
    public class TransactionsSettingsModel : IHasUserId
    {
        public TransactionsSettingsModel()
        {
            PeriodFilter = new PeriodFilterModel();
            DataFilter = new DataFilterModel();
        }

        public int UserId { get; set; }
        public bool ShowTemplates { get; set; }
        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }
    }
}
