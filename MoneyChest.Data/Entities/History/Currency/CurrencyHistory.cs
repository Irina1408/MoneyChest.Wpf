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
    public class CurrencyHistory : IUserActionHistory
    {
        public CurrencyHistory()
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
        
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public string Symbol { get; set; }

        public bool IsUsed { get; set; }

        public bool IsMain { get; set; }

        public bool SymbolAlignmentIsRight { get; set; }
    }
}
