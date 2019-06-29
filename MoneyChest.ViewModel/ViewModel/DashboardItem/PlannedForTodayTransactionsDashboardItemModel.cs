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
    public class PlannedForTodayTransactionsDashboardItemModel
    {
        public ObservableCollection<ITransaction> Entities { get; set; }

        public IMCCommand ApplyNowCommand { get; set; }
        public IMCCommand CreateTransactionCommand { get; set; }
    }
}
