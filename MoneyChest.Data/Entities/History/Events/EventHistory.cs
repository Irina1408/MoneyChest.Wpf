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
    public class EventHistory : IUserActionHistory
    {
        public EventHistory()
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

        [StringLength(1000)]
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

        [Column(TypeName = "time")]
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }
    }
}
