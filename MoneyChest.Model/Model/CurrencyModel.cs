using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
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
    public class CurrencyModel : IHasId, IHasUserId, IHasName, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CurrencyModel()
        {
            IsActive = true;
        }

        public int Id { get; set; }
        
        [StringLength(MaxSize.NameLength)]
        [Required]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        [StringLength(10)]
        public string Symbol { get; set; }

        public bool IsActive { get; set; }

        public bool IsMain { get; set; }
        
        public CurrencySymbolAlignment CurrencySymbolAlignment { get; set; }

        public int UserId { get; set; }
    }
}
