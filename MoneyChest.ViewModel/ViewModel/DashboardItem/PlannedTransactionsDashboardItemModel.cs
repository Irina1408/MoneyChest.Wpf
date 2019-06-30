using MoneyChest.Model.Model;
using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class PlannedTransactionsDashboardItemModel
    {
        public ObservableCollection<ITransaction> Entities { get; set; }

        public DateTime DateFrom { get; set; }

        public IMCCommand ApplyCommand { get; set; }
        public IMCCommand ApplyAllCommand { get; set; }
        public IMCCommand CreateTransactionCommand { get; set; }
    }
}
