using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    // Penalty for late payment
    [Historicized(typeof(DebtPenaltyHistory))]
    public class DebtPenalty : IHasId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }  // value in currency of debt

        public int DebtId { get; set; }
        public Debt Debt { get; set; }
    }
}
