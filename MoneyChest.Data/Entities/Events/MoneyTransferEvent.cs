using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Enums;
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
        public bool TakeCommissionFromReceiver { get; set; }


        public int StorageFromId { get; set; }

        public int StorageToId { get; set; }

        public int? CategoryId { get; set; }


        [ForeignKey(nameof(StorageFromId))]
        public virtual Storage StorageFrom { get; set; }

        [ForeignKey(nameof(StorageToId))]
        public virtual Storage StorageTo { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
