using MoneyChest.Model.Model;
using MoneyChest.Shared.MultiLang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.Extensions
{
    public static class ScheduleExtensions
    {
        public static string DetailedSchedule(this ScheduleModel schedule, DateTime dateFrom, DateTime? dateUntil)
        {
            // init string builder
            var sb = new StringBuilder();

            // fill main part
            switch(schedule.ScheduleType)
            {
                // once
                case Model.Enums.ScheduleType.Once:
                    return dateFrom.ToShortDateString();

                // every number of days
                case Model.Enums.ScheduleType.Daily:
                    sb.Append(schedule.Period == 1 
                        ? MultiLangResourceManager.Instance[MultiLangResourceName.EveryDay]
                        : MultiLangResource.EveryNumberDays(schedule.Period));
                    break;

                // every special day(s) of every number of weeks
                case Model.Enums.ScheduleType.Weekly:

                    if(schedule.Period == 1)
                        sb.Append(schedule.DaysOfWeek.Count == 7
                            ? MultiLangResourceManager.Instance[MultiLangResourceName.EveryDay]
                            : MultiLangResource.EveryWeek(schedule.DaysOfWeek));
                    else
                        sb.Append(schedule.DaysOfWeek.Count == 7
                            ? MultiLangResource.EveryWeek(schedule.Period)
                            : MultiLangResource.EveryWeek(schedule.DaysOfWeek, schedule.Period));
                    break;

                // every special day of every selected month
                case Model.Enums.ScheduleType.Monthly:

                    if(schedule.DayOfMonth == -1)
                        sb.Append(schedule.Months.Count == 12
                            ? MultiLangResource.EveryMonthLastDay()
                            : MultiLangResource.EveryMonthLastDay(schedule.Months));
                    else
                        sb.Append(schedule.Months.Count == 12
                        ? MultiLangResource.EveryMonth(schedule.DayOfMonth)
                        : MultiLangResource.EveryMonth(schedule.Months, schedule.DayOfMonth));

                    break;
            }

            // write date from if schedule hasn't started yet
            if (DateTime.Today < dateFrom)
            {
                sb.Append(string.Format(" {0} {1}", MultiLangResourceManager.Instance[MultiLangResourceName.FromForDate], dateFrom.ToShortDateString()));
            }

            // write date until if it is defined
            if(dateUntil.HasValue)
            {
                sb.Append(string.Format(" {0} {1}", MultiLangResourceManager.Instance[MultiLangResourceName.UntilForDate], dateUntil.Value.ToShortDateString()));
            }

            return sb.ToString();
        }
    }
}
