using MoneyChest.Data.Entities.Base;
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
    public class CalendarSetting : IHasUserId
    {
        // TODO: create CalendarFilter. Store there IsFilterApplied, StorageGroups, Storages, Categories. Apply filter after data is generated
        public CalendarSetting()
        {
            StorageGroups = new List<StorageGroup>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public CalendarPeriodType PeriodType { get; set; }

        public bool ShowLimits { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        
        // TODO: not used
        public virtual ICollection<StorageGroup> StorageGroups { get; set; }
    }
}
