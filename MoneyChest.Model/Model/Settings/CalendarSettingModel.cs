using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CalendarSettingModel : IHasUserId
    {
        public CalendarSettingModel()
        {
            PeriodType = CalendarPeriodType.Month;
            ShowLimits = false;
        }

        public int UserId { get; set; }

        public CalendarPeriodType PeriodType { get; set; }

        public bool ShowLimits { get; set; }
        
        public List<int> StorageGroupIds { get; set; }
    }
}
