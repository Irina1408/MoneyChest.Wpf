using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;

namespace MoneyChest.Model.Model
{
    public class GeneralSettingModel : IHasUserId
    {
        public int UserId { get; set; }

        public Language Language { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }
    }
}
