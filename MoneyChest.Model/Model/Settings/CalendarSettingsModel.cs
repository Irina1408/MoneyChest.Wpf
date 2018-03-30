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
    public class CalendarSettingsModel : IHasUserId
    {
        public CalendarSettingsModel()
        {
            ShowLimits = true;
            PeriodFilter = new PeriodFilterModel();
            DataFilter = new DataFilterModel();
        }

        public int UserId { get; set; }

        public bool ShowLimits { get; set; }
        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }
    }
}
