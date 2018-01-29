using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Extensions
{
    public static class ToReferenceViewExtensions
    {
        public static CurrencyReference ToReferenceView(this CurrencyModel entity)
        {
            return new CurrencyReference
            {
                Id = entity.Id,
                Name = entity.Name,
                Symbol = entity.Symbol,
                SymbolAlignment = entity.CurrencySymbolAlignment
            };
        }
        
        public static DebtReference ToReferenceView(this DebtModel entity)
        {
            return new DebtReference
            {
                Id = entity.Id,
                Description = entity.Description,
                DebtType = entity.DebtType,
                CurrencyId = entity.CurrencyId
            };
        }
    }
}
