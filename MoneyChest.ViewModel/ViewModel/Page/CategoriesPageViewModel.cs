using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    /// <summary>
    /// TODO: requres refactoring with EntityListViewModel<T>
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class CategoriesPageViewModel
    {
        public CategoryViewModelCollection Categories { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
        public IMCCommand ChangeActivityCommand { get; set; }
        public bool SelectedCategoryIsActive { get; set; }

        public IMCCommand ExpandAllCommand { get; set; }
        public IMCCommand CollapseAllCommand { get; set; }
    }
}
