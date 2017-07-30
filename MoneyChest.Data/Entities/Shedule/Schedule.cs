using MoneyChest.Data.Enums;
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
    [Historicized(typeof(ScheduleHistory))]
    public class Schedule
    {
        public Schedule()
        { 
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EventId { get; set; }


        [ForeignKey(nameof(EventId))]
        public virtual Evnt Event { get; set; }
    }
}
