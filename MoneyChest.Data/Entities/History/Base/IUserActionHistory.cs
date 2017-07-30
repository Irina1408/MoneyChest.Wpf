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
    public interface IUserActionHistory
    {
        int ActionId { get; set; }

        DateTime ActionDateTime { get; set; }

        ActionType ActionType { get; set; }
        
        int UserId { get; set; }
        
        User User { get; set; }
    }
}
