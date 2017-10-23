using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class RepayDebtEventModel : EventModel
    {
        public int StorageId { get; set; }
        public int DebtId { get; set; }

        
        public StorageReference Storage { get; set; }
        public DebtReference Debt { get; set; }
    }
}
