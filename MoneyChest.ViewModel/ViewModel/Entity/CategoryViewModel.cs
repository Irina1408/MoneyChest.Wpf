using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    public class CategoryViewModel : CategoryModel
    {
        public CategoryViewModelCollection Children { get; set; } = new CategoryViewModelCollection();
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelectedMainView { get; set; }
        public bool IsExpandedMainView { get; set; }

        public bool IsPlus => RecordType.HasValue && RecordType.Value == Model.Enums.RecordType.Income;
        public bool IsMinus => RecordType.HasValue && RecordType.Value == Model.Enums.RecordType.Expense;
        public bool HasParent => ParentCategoryId.HasValue;
    }
}
