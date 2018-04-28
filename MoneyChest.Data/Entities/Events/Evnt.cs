using MoneyChest.Data.Attributes;
using MoneyChest.Model.Base;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Constants;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(EventHistory))]
    [Table("Events")]
    public class Evnt : IHasId, IHasUserId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        public decimal Value { get; set; }

        public decimal CurrencyExchangeRate { get; set; }

        public bool TakeExistingCurrencyExchangeRate { get; set; }

        public decimal Commission { get; set; }

        public CommissionType CommissionType { get; set; }

        public EventState EventState { get; set; }

        public EventType EventType { get; set; }
        
        public string Schedule { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PausedToDate { get; set; }

        public bool AutoExecution { get; set; }

        // TODO: remove if it's unused
        [Column(TypeName = "time")]
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int UserId { get; set; }

        
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
