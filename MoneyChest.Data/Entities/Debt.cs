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
using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(DebtHistory))]
    public class Debt : IHasId, IHasUserId
    {
        public Debt()
        {
            RepayDebtEvents = new List<RepayDebtEvent>();
            Records = new List<Record>();
            DebtPenalties = new List<DebtPenalty>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        public DebtType DebtType { get; set; }

        public decimal CurrencyExchangeRate { get; set; }   // if exists StorageId -> for add/remove money to/from storage
        public decimal Value { get; set; }                  // initial value that will be added/removed to/from storage
        public decimal InitialFee { get; set; }  // initial paid value
        public decimal PaidValue { get; set; }  // paid value by user records in Money Chest
        public bool TakeInitialFeeFromStorage { get; set; }

        // payment conditions
        public DebtPaymentType PaymentType { get; set; }
        public decimal FixedAmount { get; set; }
        public decimal InterestRate { get; set; }     // Percentage
        public int MonthCount { get; set; }

        [Column(TypeName = "date")]
        public DateTime TakingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }  // user sets date when debt should be repaid. just for user notifications

        [Column(TypeName = "date")]
        public DateTime? RepayingDate { get; set; } // date of settings IsRepaid=true

        public bool IsRepaid { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }
        
        
        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int? StorageId { get; set; } 

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
        public virtual ICollection<DebtPenalty> DebtPenalties { get; set; }

        #endregion
    }
}
