﻿using MoneyChest.Data.Enums;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.History
{
    public class SimpleEventHistory : EventHistory
    {
        public RecordType RecordType { get; set; }

        public int CurrencyId { get; set; }

        public int StorageId { get; set; }

        public int? CategoryId { get; set; }
    }
}
