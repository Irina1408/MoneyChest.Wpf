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
    [Table(nameof(RecordsViewFilter))]
    public class RecordsViewFilter
    {
        public RecordsViewFilter() : base()
        {
            AllCategories = true;
            PeriodFilterType = PeriodFilterType.ThisMonth;

            Categories = new List<Category>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public bool AllCategories { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public TransactionType? TransactionType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
