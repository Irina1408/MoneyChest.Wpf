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
    public class LimitModel : IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public LimitModel()
        {
            DateFrom = DateTime.Today.AddDays(1);
            DateUntil = DateFrom.Date;
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public decimal Value { get; set; }
        public decimal SpentValue { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }
        
        public int CurrencyId { get; set; }
        public int? CategoryId { get; set; }
        public int UserId { get; set; }


        public CurrencyReference Currency { get; set; }
        public CategoryReference Category { get; set; }


        public LimitState State => 
            DateUntil > DateTime.Today ? LimitState.Closed : (DateFrom > DateTime.Today ? LimitState.Planned : LimitState.Active);
        public bool IsExceeded => RemainingValue < 0;
        public decimal RemainingValue => Value - SpentValue;
        public decimal RemainingPercent => Value > 0 ? (RemainingValue / Value * 100) : 0;
        public string ValueCurrency => Currency?.FormatValue(Value);
        public string RemainingValueCurrency => Currency?.FormatValue(RemainingValue);
        public string RemainingPercentValueCurrency => 
            $"{Currency?.FormatValue(RemainingValue)} ({Decimal.Round(RemainingPercent, 2)}%)";
    }
}
