using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(MoneyTransferTemplateHistory))]
    public class MoneyTransferTemplate : IHasId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }

        [StringLength(MaxSize.DescriptionLength)]
        public string Description { get; set; }

        public decimal Value { get; set; } // always in StorageFrom currency

        public decimal CurrencyExchangeRate { get; set; }

        public bool SwappedCurrenciesRate { get; set; }

        public decimal Commission { get; set; } // always in StorageFrom currency if CommissionType == Currency

        public CommissionType CommissionType { get; set; }

        public bool TakeCommissionFromReceiver { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }


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
