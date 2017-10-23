using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CurrencyExchangeRateModel
    {
        public decimal Rate { get; set; }


        public int CurrencyFromId { get; set; }
        public int CurrencyToId { get; set; }
        

        public CurrencyReference CurrencyFrom { get; set; }
        public CurrencyReference CurrencyTo { get; set; }
    }
}
