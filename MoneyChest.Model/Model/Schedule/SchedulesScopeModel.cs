using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class SchedulesScopeModel
    {
        public List<OnceScheduleModel> OnceSchedules { get; set; } = new List<OnceScheduleModel>();
        public List<DailyScheduleModel> DailySchedules { get; set; } = new List<DailyScheduleModel>();
        public List<WeeklyScheduleModel> WeeklySchedules { get; set; } = new List<WeeklyScheduleModel>();
        public List<MonthlyScheduleModel> MonthlySchedules { get; set; } = new List<MonthlyScheduleModel>();

        public List<int> GetEventIds()
        {
            var result = new List<int>();
            result.AddRange(OnceSchedules.Select(_ => _.EventId));
            result.AddRange(DailySchedules.Select(_ => _.EventId));
            result.AddRange(WeeklySchedules.Select(_ => _.EventId));
            result.AddRange(MonthlySchedules.Select(_ => _.EventId));

            return result.Distinct().ToList();
        }
    }
}
