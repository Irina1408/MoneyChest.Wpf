using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.History
{
    public class RepayDebtEventHistory : EventHistory
    {
        public bool ValueInStorageCurrency { get; set; }

        public int StorageId { get; set; }
        public int DebtId { get; set; }
    }
}
