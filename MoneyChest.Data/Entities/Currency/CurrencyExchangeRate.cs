using MoneyChest.Data.Attributes;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Historicized(typeof(CurrencyExchangeRateHistory))]
    public class CurrencyExchangeRate
    {
        [Key]
        [Column(Order = 1)]
        public int CurrencyFromId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CurrencyToId { get; set; }

        public decimal Rate { get; set; }

        public bool SwappedCurrencies { get; set; }


        [ForeignKey(nameof(CurrencyFromId))]
        public virtual Currency CurrencyFrom { get; set; }

        [ForeignKey(nameof(CurrencyToId))]
        public virtual Currency CurrencyTo { get; set; }
    }
}
