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
    public class LimitModel : IHasId, IHasUserId, IHasDescription, IHasRemark, INotifyPropertyChanged
    {
        private List<int> _categoryIds;

        public event PropertyChangedEventHandler PropertyChanged;

        public LimitModel()
        {
            DateFrom = DateTime.Today.AddDays(1);
            DateUntil = DateFrom.Date;
            CategoryIds = new List<int>();
        }

        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
        public decimal Value { get; set; }
        public decimal SpentValue { get; set; }

        [StringLength(MaxSize.RemarkLength)]
        public string Remark { get; set; }
        
        public List<int> CategoryIds
        {
            get => _categoryIds;
            set
            {
                _categoryIds = value;
                ActualCategoryIds = _categoryIds.Where(x => x != -1).ToList();
            }
        }

        public int CurrencyId { get; set; }
        public int UserId { get; set; }


        public CurrencyReference Currency { get; set; }

        public LimitState State => 
            DateUntil < DateTime.Today ? LimitState.Closed : (DateFrom > DateTime.Today ? LimitState.Planned : LimitState.Active);
        public bool IsExceeded => RemainingValue < 0;
        public decimal RemainingValue => Value - SpentValue;
        public decimal RemainingPercent => Value > 0 ? (RemainingValue / Value * 100) : 0;
        public string ValueCurrency => Currency?.FormatValue(Value);
        public string RemainingValueCurrency => Currency?.FormatValue(RemainingValue);
        public string RemainingPercentValueCurrency =>
            $"{Currency?.FormatValue(RemainingValue)} ({Decimal.Round(RemainingPercent, 0)}%)";

        public bool IncludeWithoutCategory => CategoryIds.Count == 0 || CategoryIds.Contains(-1);
        public bool AllCategories => CategoryIds.Count == 0;
        public List<int> ActualCategoryIds { get; set; }
    }
}
