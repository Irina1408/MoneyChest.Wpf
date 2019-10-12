using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Calendar
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class LimitState
    {
        public LimitModel Limit { get; set; }

        public List<CategoryModel> Categories { get; set; }
        public bool OneCategory => Limit.ActualCategoryIds.Count == 1;
        public bool IsNoneCategory => Limit.IncludeWithoutCategory && Limit.CategoryIds.Count == 1;
        public bool SeveralCategories => Limit.CategoryIds.Count > 1;

        public decimal SpentValue { get; set; }
        public decimal RemainingValue => Limit.Value - SpentValue;
        public string RemainingValueDetailed => Limit.Currency?.FormatValue(RemainingValue) ?? RemainingValue.ToString();
        
        public bool IsExceeded => RemainingValue < 0;
    }
}
