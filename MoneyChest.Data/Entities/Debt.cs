using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Entities.Base;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(DebtHistory))]
    public class Debt : IHasId, IHasUserId
    {
        public Debt()
        {
            RepayDebtEvents = new List<RepayDebtEvent>();
            Records = new List<Record>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        public DebtType DebtType { get; set; }
        
        public decimal Value { get; set; }
        
        public decimal PaidValue { get; set; }
        
        public bool IsRepaid { get; set; }

        [Column(TypeName = "date")]
        public DateTime TakingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RepayingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        public string Remark { get; set; }

        // TODO: regular payment
        
        public int CurrencyId { get; set; }

        public int? CategoryId { get; set; }

        public int? StorageId { get; set; } // if exists always in currency of debt

        [Required]
        public int UserId { get; set; }

        #region Navigation properties

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<RepayDebtEvent> RepayDebtEvents { get; set; }
        public virtual ICollection<Record> Records { get; set; }

        #endregion
    }
}
