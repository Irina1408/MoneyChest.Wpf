using MoneyChest.Data.Enums;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.History
{
    public class DebtHistory : IUserActionHistory
    {
        public DebtHistory()
        {
            ActionDateTime = DateTime.Now;
        }

        #region IUserActionHistory implementation

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActionId { get; set; }

        public DateTime ActionDateTime { get; set; }

        public ActionType ActionType { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        #endregion

        public int Id { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }
        public DebtType DebtType { get; set; }

        public decimal CurrencyExchangeRate { get; set; }   // if exists StorageId -> for add/remove money to/from storage
        public decimal Value { get; set; }                  // initial value that will be added/removed to/from storage
        public decimal InitialFee { get; set; }  // initial paid value -> will be removed from Value when Storage value should be changed
        public decimal PaidValue { get; set; }  // paid value by user records in Money Chest

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

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }


        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int? StorageId { get; set; }
    }
}
