﻿using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MoneyTransferModel : IHasId
    {
        public int Id { get; set; }
        
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

        
        public StorageReference StorageFrom { get; set; }
        public StorageReference StorageTo { get; set; }
    }
}
