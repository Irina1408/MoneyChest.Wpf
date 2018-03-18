using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class PeriodFilter : IHasId
    {
        public PeriodFilter()
        {
            TransactionsSettings = new List<TransactionsSettings>();
        }

        [Key]
        public int Id { get; set; }
        public PeriodType PeriodType { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateUntil { get; set; }

        public virtual ICollection<TransactionsSettings> TransactionsSettings { get; set; }
    }
}
