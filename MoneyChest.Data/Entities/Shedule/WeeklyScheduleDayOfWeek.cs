using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(WeeklyScheduleDayOfWeekHistory))]
    public class WeeklyScheduleDayOfWeek
    {
        [Key]
        [Column(Order = 1)]
        public int WeeklyScheduleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public DayOfWeek DayOfWeek { get; set; }


        [ForeignKey(nameof(WeeklyScheduleId))]
        public virtual WeeklySchedule WeeklySchedule { get; set; }
    }
}
