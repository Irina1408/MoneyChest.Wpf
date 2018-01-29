using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(RepayDebtEventHistory))]
    public class RepayDebtEvent : Evnt
    {
        public bool IsValueInStorageCurrency { get; set; }    // by default value should be in currency of debt (false)
        public int StorageId { get; set; }      // use currency exchange rate to update storage if currencies is different
        public int DebtId { get; set; }         // use currency exchange rate to update debt if currencies is different

        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }

        [ForeignKey(nameof(DebtId))]
        public virtual Debt Debt { get; set; }
    }
}
