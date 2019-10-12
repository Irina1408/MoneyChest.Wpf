using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Table(nameof(CalendarSettings))]
    public class CalendarSettings : IHasUserId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        public bool ShowSettings { get; set; }
        public bool ShowAllStorages { get; set; }
        public bool ShowAllLimits { get; set; }
        public bool ShowAllTransactionsPerDay { get; set; }
        public int MaxTransactionsCountPerDay { get; set; }
        public int DataFilterId { get; set; }
        public int PeriodFilterId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(PeriodFilterId))]
        public virtual PeriodFilter PeriodFilter { get; set; }

        [ForeignKey(nameof(DataFilterId))]
        public virtual DataFilter DataFilter { get; set; }
    }
}
