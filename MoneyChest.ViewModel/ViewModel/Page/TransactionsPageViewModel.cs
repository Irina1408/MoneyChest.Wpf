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
    public class TransactionsPageViewModel
    {
        public ObservableCollection<ITransaction> Entities { get; set; }
        public ObservableCollection<ITransaction> FilteredEntities { get; set; }
        public CategoryViewModelCollection Categories { get; set; }

        public IMCCommand AddRecordCommand { get; set; }
        public IMCCommand AddMoneyTransferCommand { get; set; }
        public IMCCommand AddChequeCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
        public IMCCommand ApplyNowCommand { get; set; }
        public IMCCommand CreateTransactionCommand { get; set; }
        public IMCCommand DuplicateAndApplyNowCommand { get; set; }
        public IMCCommand DuplicateCommand { get; set; }

        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }
    }
}
