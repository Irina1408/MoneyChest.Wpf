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
    [Historicized(typeof(OnceScheduleHistory))]
    public class OnceSchedule : Schedule
    {
        public OnceSchedule() : base()
        {
            Date = DateTime.Today.AddDays(1);
            ScheduleType = Enums.ScheduleType.Once;
        }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
    }
}
