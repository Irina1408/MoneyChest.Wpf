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

        // Hide coin box accounts in every selection
        public bool HideCoinBoxStorages { get; set; }

        public Language Language { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }

        public int DebtCategoryId { get; set; }

        // Money transfer comission category
        public int ComissionCategoryId { get; set; }
    }
}
