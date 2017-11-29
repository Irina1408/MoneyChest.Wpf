﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(MoneyTransferHistory))]
    public class MoneyTransfer : IHasId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        
        public DateTime Date { get; set; }

        public decimal Value { get; set; } // always in StorageFrom currency

        public decimal CurrencyExchangeRate { get; set; }

        public decimal Commission { get; set; } // always in StorageFrom currency if CommissionType == Currency

        public CommissionType CommissionType { get; set; }

        public bool TakeCommissionFromReceiver { get; set; }

        [StringLength(4000)]
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
