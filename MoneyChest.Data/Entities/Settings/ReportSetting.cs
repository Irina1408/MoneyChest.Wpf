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
    [Table(nameof(ReportSetting))]
    public class ReportSetting : IHasUserId
    {
        public ReportSetting()
        {
            Categories = new List<Category>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public bool IncludeRecordsWithoutCategory { get; set; }

        public bool AllCategories { get; set; }

        public ReportType ReportType { get; set; }

        public TransactionType? DataType { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public int CategoryLevel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
