﻿using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class SimpleEventModel : EventModel
    {
        public TransactionType TransactionType { get; set; }

        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int StorageId { get; set; }

        
        public CurrencyReference Currency { get; set; }
        public CategoryReference Category { get; set; }
        public StorageReference Storage { get; set; }
    }
}
