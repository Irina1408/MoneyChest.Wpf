using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ItemViewModel(object value, string description)
        {
            Value = value;
            Description = description;
        }
        
        public object Value { get; private set; }
        public string Description { get; private set; }
    }
}
