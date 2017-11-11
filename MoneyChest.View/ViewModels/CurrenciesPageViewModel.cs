using MoneyChest.View.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MoneyChest.View.ViewModels
{
    public class CurrenciesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
        public IMCCommand SetMainCommand { get; set; }
        public bool SelectedCurrenciesAreUsed { get; set; }
        public IMCCommand ChangeUsabilityCommand { get; set; }

        // TODO: ChangeUsabilityLabel and ChangeUsabilityVisibility can be replaced to special command
        public string ChangeUsabilityLabel { get; set; }
        public Visibility EnableVisibility { get; set; }
        public Visibility DisableVisibility { get; set; } = Visibility.Collapsed;
        public Visibility ChangeUsabilityVisibility { get; set; }
    }
}
