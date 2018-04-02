using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared
{
    public static class GlobalVariables
    {
        public static int UserId { get; set; }

        // TODO: load from General settings
        public static DayOfWeek FirstDayOfWeek => System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
    }
}
