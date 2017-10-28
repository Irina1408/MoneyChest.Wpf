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
    public class UserHistory : IUserActionHistory
    {
        public UserHistory()
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

        public string Name { get; set; }
        
        public string Password { get; set; }

        public DateTime FirstUsageDate { get; set; }

        public DateTime LastUsageDate { get; set; }

        public DateTime? LastSynchronizationDate { get; set; }

        public string ServerUserId { get; set; }
    }
}
