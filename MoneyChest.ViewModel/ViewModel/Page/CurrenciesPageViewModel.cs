using MoneyChest.Model.Model;
using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    /// <summary>
    /// TODO: requres refactoring with EntityListViewModel<T>
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class CurrenciesPageViewModel
    {
        public ObservableCollection<CurrencyModel> Currencies { get; set; }
        public ObservableCollection<CurrencyExchangeRateModel> CurrencyExchangeRates { get; set; }

        public IMCCommand AddCurrencyCommand { get; set; }
        public IMCCommand EditCurrencyCommand { get; set; }
        public IMCCommand DeleteCurrencyCommand { get; set; }
        public IMCCommand SetMainCommand { get; set; }
        public IMCCommand ChangeActivityCommand { get; set; }
        public bool SelectedCurrenciesAreActive { get; set; }
        
        public IMCCommand AddCurrencyExchangeRateCommand { get; set; }
        public IMCCommand EditCurrencyExchangeRateCommand { get; set; }
        public IMCCommand DeleteCurrencyExchangeRateCommand { get; set; }
    }
}
