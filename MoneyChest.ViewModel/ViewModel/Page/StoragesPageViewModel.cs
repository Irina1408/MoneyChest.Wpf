using MoneyChest.Model.Model;
using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    /// <summary>
    /// TODO: requres refactoring with EntityListViewModel<T>
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class StoragesPageViewModel
    {
        public ICommand AddStorageCommand { get; set; }
        public ICommand EditStorageCommand { get; set; }
        public ICommand DeleteStorageCommand { get; set; }
        public ICommand TransferMoneyCommand { get; set; }

        public ICommand AddStorageGroupCommand { get; set; }
        public ICommand EditStorageGroupCommand { get; set; }
        public ICommand DeleteStorageGroupCommand { get; set; }

        public ICommand AddMoneyTransferCommand { get; set; }
        public ICommand EditMoneyTransferCommand { get; set; }
        public ICommand DeleteMoneyTransferCommand { get; set; }

        public ObservableCollection<MoneyTransferModel> MoneyTransfers { get; set; }
        public List<MoneyTransferModel> FilteredMoneyTransfers { get; set; }
        public DataFilterModel MoneyTransfersDataFilter { get; set; }
        public PeriodFilterModel MoneyTransfersPeriodFilter { get; set; }
    }
}
