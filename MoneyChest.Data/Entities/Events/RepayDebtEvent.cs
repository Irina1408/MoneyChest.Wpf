using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Enums;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(RepayDebtEventHistory))]
    public class RepayDebtEvent : Evnt
    {
        public RepayDebtEvent() : base()
        {
            EventType = EventType.RepayDebt;
        }

        public int StorageId { get; set; }  // take currency from storage

        public int DebtId { get; set; }     // currency of debt should be the same of currency of storage


        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }

        [ForeignKey(nameof(DebtId))]
        public virtual Debt Debt { get; set; }
    }
}
