using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Utils
{
    public static class PeriodUtils
    {
        public static string GetPeriodRangeDetails(PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            switch (periodType)
            {
                case PeriodType.Day:
                    return dateFrom.ToShortDateString();

                case PeriodType.Month:
                    return $"{dateFrom.ToString("MMMM")} {dateFrom.Year}";

                case PeriodType.Quarter:
                    return $"Q{(dateFrom.Month - 1) / 3 + 1} {dateFrom.Year}";

                case PeriodType.Year:
                    return dateFrom.Year.ToString();

                default:
                    return $"{dateFrom.ToShortDateString()} - {dateUntil.ToShortDateString()}";
            }
        }

        public static DateRange GetNextDateRange(PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            DateTime date;

            switch (periodType)
            {
                case PeriodType.Month:
                    date = dateFrom.AddMonths(1);
                    return new DateRange(date, date.AddMonths(1).AddMilliseconds(-1));

                case PeriodType.Quarter:
                    date = dateFrom.AddMonths(3);
                    return new DateRange(date, date.AddMonths(3).AddMilliseconds(-1));

                case PeriodType.Year:
                    date = dateFrom.AddYears(1);
                    return new DateRange(date, date.AddYears(1).AddMilliseconds(-1));

                default:
                    var diff = (dateUntil - dateFrom).Days + 1;
                    return new DateRange(dateFrom.AddDays(diff), dateUntil.AddDays(diff));
            }
        }

        public static DateRange GetPrevDateRange(PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            DateTime date;

            switch (periodType)
            {
                case PeriodType.Month:
                    date = dateFrom.AddMonths(-1);
                    return new DateRange(date, date.AddMonths(1).AddMilliseconds(-1));

                case PeriodType.Quarter:
                    date = dateFrom.AddMonths(-3);
                    return new DateRange(date, date.AddMonths(3).AddMilliseconds(-1));

                case PeriodType.Year:
                    date = dateFrom.AddYears(-1);
                    return new DateRange(date, date.AddYears(1).AddMilliseconds(-1));

                default:
                    var diff = (dateUntil - dateFrom).Days + 1;
                    return new DateRange(dateFrom.AddDays(-diff), dateUntil.AddDays(-diff));
            }
        }

        public static List<DateRange> SplitDateRange(PeriodType periodType, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<DateRange>();
            // populate all periods that exists in the provided date range
            var startDate = dateFrom.Date;
            var endDate = dateFrom.Date;
            while(startDate < dateUntil.Date)
            {
                switch (periodType)
                {
                    case PeriodType.Day:
                        endDate = startDate.Date;
                        break;

                    case PeriodType.Week:
                        endDate = startDate.FirstDayOfWeek().AddDays(6);
                        break;

                    case PeriodType.Month:
                        endDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                        break;

                    case PeriodType.Quarter:
                        endDate = startDate.FirstDayOfQuater().AddMonths(3);
                        break;

                    case PeriodType.Year:
                        endDate = new DateTime(startDate.Year, 12, DateTime.DaysInMonth(startDate.Year, 12));
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                // make sure end date doen't exceed end day of date range
                if(endDate > dateUntil.Date)
                    endDate = dateUntil.Date;
                // adapt end date
                endDate = endDate.AddDays(1).AddMilliseconds(-1);
                // populate result
                result.Add(new DateRange(startDate, endDate));
                // next range
                startDate = endDate.Date.AddDays(1);
            }

            return result;
        }
    }
}
