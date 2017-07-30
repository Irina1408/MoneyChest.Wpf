using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(MoneyTransferHistory))]
    public class MoneyTransfer
    {
        public MoneyTransfer()
        {
            CurrencyExchangeRate = 1;
            TakeComissionFromReceiver = false;
            TakeComissionCurrencyFromReceiver = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public decimal Value { get; set; }

        public decimal CurrencyExchangeRate { get; set; }

        public decimal Commission { get; set; }

        public CommissionType? CommissionType { get; set; }

        public bool TakeComissionFromReceiver { get; set; }

        public bool TakeComissionCurrencyFromReceiver { get; set; }

        public string Remark { get; set; }


        public int StorageFromId { get; set; }

        public int StorageToId { get; set; }


        [ForeignKey(nameof(StorageFromId))]
        public virtual Storage StorageFrom { get; set; }

        [ForeignKey(nameof(StorageToId))]
        public virtual Storage StorageTo { get; set; }
    }
}
