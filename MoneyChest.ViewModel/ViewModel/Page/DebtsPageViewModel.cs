using MoneyChest.Model.Model;
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
    public class DebtsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<DebtModel> Debts { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
    }
}
