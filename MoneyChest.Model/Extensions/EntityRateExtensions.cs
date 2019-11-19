using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Extensions
{
    public static class EntityRateExtensions
    {
        public static decimal ActualRate(this IHasExchangeRate rate)
        {
            if (rate.SwappedCurrenciesRate && rate.CurrencyExchangeRate != 0)
                return 1M / rate.CurrencyExchangeRate;
            else
                return rate.CurrencyExchangeRate;
        }
    }
}
