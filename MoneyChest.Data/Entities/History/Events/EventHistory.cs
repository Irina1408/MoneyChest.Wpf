using MoneyChest.Data.Enums;
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

        public string Description { get; set; }

        public decimal Value { get; set; }

        public EventState EventState { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PausedToDate { get; set; }

        public EventType EventType { get; set; }

        public bool AutoExecution { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }

        public string Remark { get; set; }
    }
}
