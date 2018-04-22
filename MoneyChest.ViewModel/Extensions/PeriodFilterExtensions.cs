using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.Extensions
{
    public static class PeriodFilterExtensions
    {
        public static void NextDateRange(this PeriodFilterModel periodFilter)
        {
            periodFilter.IsDateRangeFilling = true;

            switch (periodFilter.PeriodType)
            {
                case PeriodType.Month:
                    periodFilter.DateFrom = periodFilter.DateFrom.AddMonths(1);
                    periodFilter.DateUntil = periodFilter.DateFrom.AddMonths(1).AddMilliseconds(-1);
                    break;

                case PeriodType.Quarter:
                    periodFilter.DateFrom = periodFilter.DateFrom.AddMonths(3);
                    periodFilter.DateUntil = periodFilter.DateFrom.AddMonths(3).AddMilliseconds(-1);
                    break;

                case PeriodType.Year:
                    periodFilter.DateFrom = periodFilter.DateFrom.AddYears(1);
                    periodFilter.DateUntil = periodFilter.DateFrom.AddYears(1).AddMilliseconds(-1);
                    break;

                default:
                    var diff = (periodFilter.DateUntil - periodFilter.DateFrom).Days + 1;
                    periodFilter.DateFrom = periodFilter.DateFrom.AddDays(diff);
                    periodFilter.DateUntil = periodFilter.DateUntil.AddDays(diff);
                    break;
            }

            periodFilter.IsDateRangeFilling = false;
        }

        public static void PrevDateRange(this PeriodFilterModel periodFilter)
        {
            periodFilter.IsDateRangeFilling = true;

            switch (periodFilter.PeriodType)
            {
                case PeriodType.Month:
                    periodFilter.DateFrom = periodFilter.DateFrom.AddMonths(-1);
                    periodFilter.DateUntil = periodFilter.DateFrom.AddMonths(1).AddMilliseconds(-1);
                    break;

                case PeriodType.Quarter:
                    periodFilter.DateFrom = periodFilter.DateFrom.AddMonths(-3);
                    periodFilter.DateUntil = periodFilter.DateFrom.AddMonths(3).AddMilliseconds(-1);
                    break;

                case PeriodType.Year:
                    periodFilter.DateFrom = periodFilter.DateFrom.AddYears(-1);
                    periodFilter.DateUntil = periodFilter.DateFrom.AddYears(1).AddMilliseconds(-1);
                    break;

                default:
                    var diff = (periodFilter.DateUntil - periodFilter.DateFrom).Days + 1;
                    periodFilter.DateFrom = periodFilter.DateFrom.AddDays(-diff);
                    periodFilter.DateUntil = periodFilter.DateUntil.AddDays(-diff);
                    break;
            }
            periodFilter.IsDateRangeFilling = false;
        }
    }
}
