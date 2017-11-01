using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;

namespace MoneyChest.Model.Model
{
    public class WeeklyScheduleModel : ScheduleModel
    {
        public WeeklyScheduleModel() : base()
        {
            Period = 1;
            DateFrom = DateTime.Today.AddDays(1);
            ScheduleType = ScheduleType.Weekly;

            DaysOfWeek = new List<DayOfWeek>();
        }

        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public int Period { get; set; }

        public List<DayOfWeek> DaysOfWeek { get; set; }
    }
}
