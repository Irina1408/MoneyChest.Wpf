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
    public class CategoriesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryViewModelCollection Categories { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
        public IMCCommand ChangeActivityCommand { get; set; }
        public bool SelectedCategoryIsActive { get; set; }
        // TODO: add expandAll/collapseAll commands
    }
}
