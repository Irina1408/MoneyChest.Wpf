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
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class TransactionsPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Transactions part
        public ObservableCollection<ITransaction> TransactionEntities { get; set; }
        public List<ITransaction> TransactionFilteredEntities { get; set; }

        public IMCCommand AddRecordCommand { get; set; }
        public IMCCommand AddMoneyTransferCommand { get; set; }
        public IMCCommand AddChequeCommand { get; set; }
        public IMCCommand EditTransactionCommand { get; set; }
        public IMCCommand DeleteTransactionCommand { get; set; }
        public IMCCommand ApplyNowPlannedTransactionCommand { get; set; }
        public IMCCommand CreateTransactionFromPlannedCommand { get; set; }
        public IMCCommand DuplicateAndApplyNowTransactionCommand { get; set; }
        public IMCCommand DuplicateTransactionCommand { get; set; }
        public IMCCommand CreateTemplateFromTransactionCommand { get; set; }

        // Transaction filters
        public PeriodFilterModel PeriodFilter { get; set; }

        // Templates part
        public ObservableCollection<ITransactionTemplate> TemplateEntities { get; set; }

        public IMCCommand AddRecordTemplateCommand { get; set; }
        public IMCCommand AddMoneyTransferTemplateCommand { get; set; }
        public IMCCommand EditTemplateCommand { get; set; }
        public IMCCommand DeleteTemplateCommand { get; set; }

        public IMCCommand ApplyNowTemplateCommand { get; set; }
        public IMCCommand CreateTransactionFromTemplateCommand { get; set; }

        // Common & Transaction filters
        public bool ShowTemplates { get; set; }
        public DataFilterModel DataFilter { get; set; }
    }
}
