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
    public class AutoExecutedTransactionsDashboardItemModel
    {
        public ObservableCollection<ITransaction> Entities { get; set; }

        public DateTime DateFrom { get; set; }

        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
    }
}
