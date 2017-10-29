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
    [Historicized(typeof(SimpleEventHistory))]
    public class SimpleEvent : Evnt
    {
        public TransactionType TransactionType { get; set; }

        public int CurrencyId { get; set; }

        public int? CategoryId { get; set; }

        public int StorageId { get; set; }


        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }

        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }
    }
}
