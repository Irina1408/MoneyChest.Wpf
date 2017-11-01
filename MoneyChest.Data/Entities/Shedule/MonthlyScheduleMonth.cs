using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(MonthlyScheduleMonthHistory))]
    public class MonthlyScheduleMonth
    {
        [Key]
        [Column(Order = 1)]
        public int MonthlyScheduleId { get; set; }

        [Key]
        [Column(Order = 2)]
        public Month Month { get; set; }


        [ForeignKey(nameof(MonthlyScheduleId))]
        public virtual MonthlySchedule MonthlySchedule { get; set; }
    }
}
