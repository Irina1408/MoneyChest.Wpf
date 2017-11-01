using MoneyChest.Data.Enums;
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

        public string Description { get; set; }

        public DebtType DebtType { get; set; }

        [Column(TypeName = "date")]
        public DateTime TakingDate { get; set; }

        public decimal Value { get; set; }

        public decimal PaidValue { get; set; }

        public bool IsRepaid { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RepayingDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DueDate { get; set; }

        public string Remark { get; set; }


        public int CurrencyId { get; set; }

        public int? StorageId { get; set; }
    }
}
