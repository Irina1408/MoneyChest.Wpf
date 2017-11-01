using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(ScheduleHistory))]
    public class Schedule : IHasId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ScheduleType ScheduleType { get; set; }

        public int EventId { get; set; }


        [ForeignKey(nameof(EventId))]
        public virtual Evnt Event { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual MoneyTransferEvent MoneyTransferEvent { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual RepayDebtEvent RepayDebtEvent { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual SimpleEvent SimpleEvent { get; set; }
    }
}
