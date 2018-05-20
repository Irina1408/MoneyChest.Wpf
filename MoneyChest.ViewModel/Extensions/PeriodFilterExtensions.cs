using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Model.Utils;
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

            var dateRange = PeriodUtils.GetNextDateRange(periodFilter.PeriodType, periodFilter.DateFrom, periodFilter.DateUntil);
            periodFilter.DateFrom = dateRange.DateFrom;
            periodFilter.DateUntil = dateRange.DateUntil;

            periodFilter.IsDateRangeFilling = false;
        }

        public static void PrevDateRange(this PeriodFilterModel periodFilter)
        {
            periodFilter.IsDateRangeFilling = true;

            var dateRange = PeriodUtils.GetPrevDateRange(periodFilter.PeriodType, periodFilter.DateFrom, periodFilter.DateUntil);
            periodFilter.DateFrom = dateRange.DateFrom;
            periodFilter.DateUntil = dateRange.DateUntil;

            periodFilter.IsDateRangeFilling = false;
        }
    }
}
