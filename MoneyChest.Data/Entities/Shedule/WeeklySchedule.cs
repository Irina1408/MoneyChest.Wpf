using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(WeeklyScheduleHistory))]
    public class WeeklySchedule : Schedule
    {
        public WeeklySchedule() : base()
        {
            Period = 1;
            DateFrom = DateTime.Today.AddDays(1);
            ScheduleType = Enums.ScheduleType.Weekly;

            WeeklyScheduleDaysOfWeek = new List<WeeklyScheduleDayOfWeek>();
        }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }

        public int Period { get; set; }

        public virtual ICollection<WeeklyScheduleDayOfWeek> WeeklyScheduleDaysOfWeek { get; set; }
    }
}
