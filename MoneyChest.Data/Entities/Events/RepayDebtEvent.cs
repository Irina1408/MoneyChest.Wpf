using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(RepayDebtEventHistory))]
    public class RepayDebtEvent : Evnt
    {
        public int StorageId { get; set; }

        public int DebtId { get; set; }


        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }

        [ForeignKey(nameof(DebtId))]
        public virtual Debt Debt { get; set; }
    }
}
