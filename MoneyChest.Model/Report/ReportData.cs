using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Report
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ReportData
    {
        public CurrencyReference MainCurrency { get; set; }
        public List<CurrencyExchangeRateModel> Rates { get; set; } = new List<CurrencyExchangeRateModel>();
        public List<CategoryModel> Categories { get; set; }
    }
}
