using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Base;
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
    public class RecordTemplateHistory : IUserActionHistory
    {
        public RecordTemplateHistory()
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

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        public RecordType RecordType { get; set; }

        public decimal Value { get; set; }

        public decimal CurrencyExchangeRate { get; set; }

        public bool SwappedCurrenciesRate { get; set; }

        public decimal Commission { get; set; }

        public CommissionType CommissionType { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        public int? CategoryId { get; set; }

        public int CurrencyId { get; set; }

        public int StorageId { get; set; }

        public int? DebtId { get; set; }
    }
}
