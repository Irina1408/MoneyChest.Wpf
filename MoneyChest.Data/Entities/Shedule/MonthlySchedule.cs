using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(MonthlyScheduleHistory))]
    public class MonthlySchedule : Schedule
    {
        public MonthlySchedule() : base()
        {
            DateFrom = DateTime.Today.AddDays(1);
            ScheduleType = Enums.ScheduleType.Monthly;

            MonthlyScheduleMonths = new List<MonthlyScheduleMonth>();
        }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }

        public int DayOfMonth { get; set; }     // -1 if it's the last day of month

        public virtual ICollection<MonthlyScheduleMonth> MonthlyScheduleMonths { get; set; }
    }
}
