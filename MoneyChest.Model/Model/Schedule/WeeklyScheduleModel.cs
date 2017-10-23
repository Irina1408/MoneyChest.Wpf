using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class WeeklyScheduleModel : ScheduleModel
    {
        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public int Period { get; set; }

        public List<DayOfWeek> DaysOfWeek { get; set; } = new List<DayOfWeek>();
    }
}
