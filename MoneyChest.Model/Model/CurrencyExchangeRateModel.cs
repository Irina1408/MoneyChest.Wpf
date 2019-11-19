using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CurrencyExchangeRateModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CurrencyExchangeRateModel()
        {
            Rate = 1;
        }

        public decimal Rate { get; set; }
        public bool SwappedCurrencies { get; set; }

        public int CurrencyFromId { get; set; }
        public int CurrencyToId { get; set; }
        

        public CurrencyReference CurrencyFrom { get; set; }
        public CurrencyReference CurrencyTo { get; set; }

        public decimal ActualRate => SwappedCurrencies && Rate != 0 ? 1M / Rate : Rate;
        public string ExchangeRateExample => CurrencyFrom != null && CurrencyTo != null
            ? (!SwappedCurrencies
                ? $"{CurrencyFrom.FormatValue(1)} = {CurrencyTo.FormatRequiredDecimalsValue(Rate)}"
                : $"{CurrencyTo.FormatValue(1)} = {CurrencyFrom.FormatRequiredDecimalsValue(Rate)}")
            : null;
    }
}
