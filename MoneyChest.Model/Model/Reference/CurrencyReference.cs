using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CurrencyReference
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public CurrencySymbolAlignment SymbolAlignment { get; set; }

        public string FormatValue(decimal value, bool showSign = false) => SymbolAlignment == Enums.CurrencySymbolAlignment.Right
            ? $"{value.ToString(showSign ? "+0.##;-0.##;0" : "0.##")}{Symbol}"
            : $"{value.ToString(showSign ? $"+ {Symbol}0.##;- {Symbol}0.##;0" : $"{Symbol}0.##;- {Symbol}0.##;0")}";

        public string FormatRequiredDecimalsValue(decimal value, bool showSign = false) => 
            SymbolAlignment == Enums.CurrencySymbolAlignment.Right
            ? $"{value.ToString(showSign ? "+0.00;-0.00;0" : "0.00")}{Symbol}"
            : $"{value.ToString(showSign ? $"+ {Symbol}0.00;- {Symbol}0.00;0" : $"{Symbol}0.00;- {Symbol}0.00;0")}";
    }
}
