using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Calendar
{
    public class CalendarData
    {
        public CurrencyReference MainCurrency { get; set; }
        public List<CurrencyExchangeRateModel> Rates { get; set; } = new List<CurrencyExchangeRateModel>();
        public List<StorageModel> Storages { get; set; } = new List<StorageModel>();
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
    }
}
