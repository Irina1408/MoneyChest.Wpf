using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared
{
    public static class DateTimeExtensions
    {
        public static DateTime FirstDayOfWeek(this DateTime date, DayOfWeek? firstDayOfWeek = null)
        {
            if (!firstDayOfWeek.HasValue)
                firstDayOfWeek = System.Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            var diff = date.DayOfWeek - firstDayOfWeek.Value;

            if (diff < 0)
                diff += 7;

            return date.AddDays(-1 * diff).Date;
        }
    }
}
