using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Common
{
    public static class CalculationHelper
    {
        public static decimal ConvertToCurrency(decimal value, int currencyId, int targetCurrencyId, 
            IEnumerable<CurrencyExchangeRateModel> rates)
        {
            if (currencyId == targetCurrencyId) return value;
            var rate = rates.FirstOrDefault(item => item.CurrencyFromId == currencyId && item.CurrencyToId == targetCurrencyId);
            return value * (rate != null ? rate.Rate : 1);
        }
    }
}
