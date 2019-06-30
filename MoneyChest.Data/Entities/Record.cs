using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(RecordHistory))]
    public class Record : IHasId, IHasUserId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        public RecordType RecordType { get; set; }

        public decimal Value { get; set; }

        public decimal CurrencyExchangeRate { get; set; }

        public decimal Commission { get; set; }

        public CommissionType CommissionType { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        public bool IsAutoExecuted { get; set; }

        public int? CategoryId { get; set; }

        public int CurrencyId { get; set; }

        public int StorageId { get; set; }

        public int? DebtId { get; set; }

        public int? EventId { get; set; }

        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }

        [ForeignKey(nameof(DebtId))]
        public virtual Debt Debt { get; set; }

        [ForeignKey(nameof(EventId))]
        public virtual Evnt Event { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
