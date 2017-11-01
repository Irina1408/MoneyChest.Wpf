using MoneyChest.Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CurrencyModel : IHasId, IHasUserId
    {
        public CurrencyModel()
        {
            IsUsed = true;
            SymbolAlignmentIsRight = true;
        }

        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Code { get; set; }
        
        public string Symbol { get; set; }

        public bool IsUsed { get; set; }

        public bool IsMain { get; set; }

        public bool SymbolAlignmentIsRight { get; set; }
        
        public int UserId { get; set; }
    }
}
