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

        public string FormatValue(decimal value) => SymbolAlignment == Enums.CurrencySymbolAlignment.Right
            ? $"{value.ToString("0.##")}{Symbol}"
            : $"{Symbol}{value.ToString("0.##")}";
    }
}
