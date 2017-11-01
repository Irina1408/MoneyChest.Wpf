﻿using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MoneyTransferEventModel : EventModel
    {
        public MoneyTransferEventModel() : base()
        {
            TakeExistingCurrencyExchangeRate = true;
            CurrencyExchangeRate = 1;
            TakeComissionFromReceiver = false;
            EventType = EventType.MoneyTransfer;
            CommissionType = CommissionType.Currency;
        }

        public bool TakeExistingCurrencyExchangeRate { get; set; }
        public decimal CurrencyExchangeRate { get; set; }
        public decimal Commission { get; set; }
        public bool TakeComissionFromReceiver { get; set; }
        public CommissionType CommissionType { get; set; }


        public int StorageFromId { get; set; }
        public int StorageToId { get; set; }
        public int? CategoryId { get; set; }


        public StorageReference StorageFrom { get; set; }
        public StorageReference StorageTo { get; set; }
        public CurrencyReference StorageFromCurrency { get; set; }
        public CurrencyReference StorageToCurrency { get; set; }
        public CategoryReference Category { get; set; }

        public decimal CommisionValue => CommissionType == CommissionType.Currency ? Commission : Commission * Value;
    }
}