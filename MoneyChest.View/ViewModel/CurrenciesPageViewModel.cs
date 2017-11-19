using MoneyChest.Model.Model;
using MoneyChest.View.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MoneyChest.View.ViewModel
{
    public class CurrenciesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CurrencyModel> Currencies { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
        public IMCCommand SetMainCommand { get; set; }
        public IMCCommand ChangeActivityCommand { get; set; }
        public bool SelectedCurrenciesAreActive { get; set; }
    }
}
