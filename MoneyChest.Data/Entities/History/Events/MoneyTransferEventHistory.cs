using MoneyChest.Data.Enums;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.History
{
    public class MoneyTransferEventHistory : EventHistory
    {
        public bool TakeCommissionFromReceiver { get; set; }


        public int StorageFromId { get; set; }

        public int StorageToId { get; set; }

        public int? CategoryId { get; set; }
    }
}
