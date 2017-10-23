using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(MoneyTransferEventHistory))]
    public class MoneyTransferEvent : Evnt
    {
        public MoneyTransferEvent() : base()
        {
            TakeExistingCurrencyExchangeRate = true;
            CurrencyExchangeRate = 1;
            TakeComissionFromReceiver = false;
            TakeComissionCurrencyFromReceiver = false;
            EventType = EventType.MoneyTransfer;
        }

        public bool TakeExistingCurrencyExchangeRate { get; set; }

        public decimal CurrencyExchangeRate { get; set; }

        public decimal Commission { get; set; }

        public bool TakeComissionFromReceiver { get; set; }

        public bool TakeComissionCurrencyFromReceiver { get; set; }

        public CommissionType? CommissionType { get; set; }


        public int StorageFromId { get; set; }

        public int StorageToId { get; set; }


        [ForeignKey(nameof(StorageFromId))]
        public virtual Storage StorageFrom { get; set; }

        [ForeignKey(nameof(StorageToId))]
        public virtual Storage StorageTo { get; set; }
    }
}
