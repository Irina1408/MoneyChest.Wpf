using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.History
{
    public class MoneyTransferEventHistory : EventHistory
    {
        public bool TakeExistingCurrencyExchangeRate { get; set; }

        public decimal CurrencyExchangeRate { get; set; }

        public decimal Commission { get; set; }

        public bool TakeComissionFromReceiver { get; set; }

        public bool TakeComissionCurrencyFromReceiver { get; set; }

        public CommissionType? CommissionType { get; set; }


        public int StorageFromId { get; set; }

        public int StorageToId { get; set; }
    }
}
