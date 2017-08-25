using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    public static class CalculationHelper
    {
        public static decimal ConvertToMainCurrency(decimal value, int currencyId, int mainCurrencyId, IEnumerable<CurrencyExchangeRate> rates)
        {
            if (currencyId == mainCurrencyId) return value;
            var rate = rates.FirstOrDefault(item => item.CurrencyFromId == currencyId && item.CurrencyToId == mainCurrencyId);
            return value * (rate != null ? rate.Rate : 1);
        }
    }
}
