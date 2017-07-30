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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public StoreType? StoreType { get; set; }

        public CalendarPeriodType PeriodType { get; set; }

        public bool ShowLimits { get; set; }
        
        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
