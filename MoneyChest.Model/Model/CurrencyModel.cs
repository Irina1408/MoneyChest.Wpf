using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CurrencyModel : IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool symbolAlignmentIsRight;
        private CurrencySymbolAlignment currencySymbolAlignment;

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

        // TODO: remove and replace CurrencySymbolAlignment to database
        public bool SymbolAlignmentIsRight
        {
            get => symbolAlignmentIsRight;
            set
            {
                symbolAlignmentIsRight = value;
                currencySymbolAlignment = symbolAlignmentIsRight ? CurrencySymbolAlignment.Right : CurrencySymbolAlignment.Left;
            }
        }

        // TODO: replace CurrencySymbolAlignment to database
        public CurrencySymbolAlignment CurrencySymbolAlignment
        {
            get => currencySymbolAlignment;
            set
            {
                currencySymbolAlignment = value;
                symbolAlignmentIsRight = currencySymbolAlignment == CurrencySymbolAlignment.Right;
            }
        }

        public int UserId { get; set; }
    }
}
