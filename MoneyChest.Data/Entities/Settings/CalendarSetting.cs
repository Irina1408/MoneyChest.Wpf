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
    public class CalendarSetting
    {
        public CalendarSetting()
        {
            PeriodType = CalendarPeriodType.Month;
            ShowLimits = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public int? StorageGroupId { get; set; }

        public CalendarPeriodType PeriodType { get; set; }

        public bool ShowLimits { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(StorageGroupId))]
        public StorageGroup StorageGroup { get; set; }
    }
}
