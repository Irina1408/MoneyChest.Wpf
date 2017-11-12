using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        
        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(10)]
        public string Symbol { get; set; }

        public bool IsUsed { get; set; }

        public bool IsMain { get; set; }
        
        public bool SymbolAlignmentIsRight
        {
            get => symbolAlignmentIsRight;
            set
            {
                symbolAlignmentIsRight = value;
                currencySymbolAlignment = symbolAlignmentIsRight ? CurrencySymbolAlignment.Right : CurrencySymbolAlignment.Left;
            }
        }
        
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
