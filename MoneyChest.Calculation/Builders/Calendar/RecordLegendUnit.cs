using MoneyChest.Calculation.Common;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class RecordLegendUnit //: LegendUnit
    {
        public string Description { get; set; }
        public decimal Value { get; set; }
        public CurrencyReference Currency { get; set; }

        public TransactionType TransactionType { get; set; }
        public StorageReference Storage { get; set; }
        public CategoryReference Category { get; set; }
        public bool IsPlanned { get; set; } = false;

        // TODO: add data to get 
    }
}
