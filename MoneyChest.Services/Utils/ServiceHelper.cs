using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Utils
{
    internal static class ServiceHelper
    {
        public static Tuple<DateTime, DateTime> GetPeriod(PeriodFilterType period, DayOfWeek firstDayOfWeek)
        {
            int tmp;

            switch (period)
            {
                case PeriodFilterType.Today:
                    return new Tuple<DateTime, DateTime>(DateTime.Today, DateTime.Today.AddDays(1));

                case PeriodFilterType.Yeasterday:
                    return new Tuple<DateTime, DateTime>(DateTime.Today.AddDays(-1), DateTime.Today.AddMilliseconds(-1));

                case PeriodFilterType.ThisWeek:
                    // the past number of days this week
                    tmp = (int)DateTime.Today.DayOfWeek >= (int)firstDayOfWeek
                        ? (int)DateTime.Today.DayOfWeek - (int)firstDayOfWeek
                        : 7 - (int)firstDayOfWeek + (int)DateTime.Today.DayOfWeek;

                    return new Tuple<DateTime, DateTime>(DateTime.Today.AddDays(-tmp), DateTime.Today.AddDays(1));

                case PeriodFilterType.PreviousWeek:
                    // the past number of days this week
                    tmp = (int)DateTime.Today.DayOfWeek >= (int)firstDayOfWeek
                        ? (int)DateTime.Today.DayOfWeek - (int)firstDayOfWeek
                        : 7 - (int)firstDayOfWeek + (int)DateTime.Today.DayOfWeek;

                    return new Tuple<DateTime, DateTime>(
                        DateTime.Today.AddDays(-tmp - 7), 
                        DateTime.Today.AddDays(-tmp).AddMilliseconds(-1));

                case PeriodFilterType.ThisMonth:
                    return new Tuple<DateTime, DateTime>(
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), 
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)));

                case PeriodFilterType.PreviousMonth:
                    int year = DateTime.Today.Month > 1 ? DateTime.Today.Year : DateTime.Today.Year - 1;
                    int month = DateTime.Today.Month > 1 ? DateTime.Today.Month - 1 : 12;

                    return new Tuple<DateTime, DateTime>(
                        new DateTime(year, month, 1), 
                        new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMilliseconds(-1));

                case PeriodFilterType.ThisYear:
                    return new Tuple<DateTime, DateTime>(
                        new DateTime(DateTime.Today.Year, 1, 1), 
                        new DateTime(DateTime.Today.Year + 1, 1, 1).AddMilliseconds(-1));

                case PeriodFilterType.PreviousYear:
                    return new Tuple<DateTime, DateTime>(
                        new DateTime(DateTime.Today.Year - 1, 1, 1),
                        new DateTime(DateTime.Today.Year, 1, 1).AddMilliseconds(-1));

                default:
                    return null;
            }
        }
    }
}
