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

        public IMCCommand AddRecordCommand { get; set; }
        public IMCCommand AddMoneyTransferCommand { get; set; }
        public IMCCommand AddChequeCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }

        public IMCCommand PrevDateRangeCommand { get; set; }
        public IMCCommand NextDateRangeCommand { get; set; }
        public IMCCommand SelectDateRangeCommand { get; set; }

        public PeriodFilterModel ViewSettings { get; set; }
    }
}
