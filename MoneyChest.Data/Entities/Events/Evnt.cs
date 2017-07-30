using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(EventHistory))]
    [Table("Events")]
    public class Evnt
    {
        public Evnt()
        {
            EventState = EventState.Active;
            AutoExecution = false;
            ConfirmBeforeExecute = false;

            Schedules = new List<Schedule>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public EventState EventState { get; set; }
        
        [Column(TypeName = "date")]
        public DateTime? PausedToDate { get; set; }
        
        public ScheduleType ScheduleType { get; set; }

        public bool AutoExecution { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }

        public string Remark { get; set; }

        [Required]
        public int UserId { get; set; }

        
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
