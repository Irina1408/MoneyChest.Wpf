﻿using MoneyChest.Model.Base;
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
    public class StorageModel : IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        
        public decimal Value { get; set; }

        public bool IsVisible { get; set; } = true;

        [StringLength(4000)]
        public string Remark { get; set; }

        [Required]
        public int StorageGroupId { get; set; }
        [Required]
        public int CurrencyId { get; set; }
        public int UserId { get; set; }

        
        public virtual CurrencyReference Currency { get; set; }
        public virtual StorageGroupReference StorageGroup { get; set; }

        public string ValueCurrency => Currency.SymbolAlignment == Enums.CurrencySymbolAlignment.Right 
            ? $"{Value.ToString("0.##")}{Currency.Symbol}"
            : $"{Currency.Symbol}{Value.ToString("0.##")}";
    }
}
