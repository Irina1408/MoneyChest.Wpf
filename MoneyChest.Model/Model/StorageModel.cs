using MoneyChest.Model.Base;
using MoneyChest.Model.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class StorageModel : IHasId, IHasUserId, IHasName, IHasRemark, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Id { get; set; }

        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }
        
        public decimal Value { get; set; }

        public bool IsVisible { get; set; } = true;

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }

        [Required]
        public int StorageGroupId { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public int UserId { get; set; }

        
        public virtual CurrencyReference Currency { get; set; }
        public virtual StorageGroupReference StorageGroup { get; set; }

        public string ValueCurrency => Currency?.FormatValue(Value);
        public string NameValue => Currency != null ? $"{Name} ({ValueCurrency})" : Name;
    }
}
