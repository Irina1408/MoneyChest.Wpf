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
    [Historicized(typeof(DailyScheduleHistory))]
    public class DailySchedule : Schedule
    {
        public DailySchedule() : base()
        {
            Period = 1;
            DateFrom = DateTime.Today.AddDays(1);
            ScheduleType = Enums.ScheduleType.Daily;
        }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }

        public int Period { get; set; }
    }
}
