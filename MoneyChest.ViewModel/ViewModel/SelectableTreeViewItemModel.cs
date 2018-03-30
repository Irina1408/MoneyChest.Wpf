using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    //[PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SelectableTreeViewItemModel<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public T Value { get; set; }
    }
}
