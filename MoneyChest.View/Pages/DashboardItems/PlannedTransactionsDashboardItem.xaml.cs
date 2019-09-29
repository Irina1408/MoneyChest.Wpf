using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyChest.View.Pages.DashboardItems
{
    /// <summary>
    /// Interaction logic for PlannedTransactionsDashboardItem.xaml
    /// </summary>
    public partial class PlannedTransactionsDashboardItem : UserControl, IDashboardItem
    {
        #region Private fields

        private PlannedTransactionsDashboardItemModel _viewModel;
        private ITransactionService _service;
        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;

        #endregion

        #region Initialization

        public PlannedTransactionsDashboardItem()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _recordService = ServiceManager.ConfigureService<RecordService>();
            _moneyTransferService = ServiceManager.ConfigureService<MoneyTransferService>();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new PlannedTransactionsDashboardItemModel()
            {
                DateFrom = GlobalVariables.LastUsageDate < DateTime.Today ? GlobalVariables.LastUsageDate.AddDays(1) : DateTime.Today,

                ApplyCommand = new DataGridSelectedItemsCommand<ITransaction>(GridTransactions, ApplyTransactions),

                ApplyAllCommand = new Command(() => ApplyTransactions(_viewModel.Entities)),

                CreateTransactionCommand = new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    var plannedTransaction = item as PlannedTransactionModel<EventModel>;

                    // simple event
                    if (plannedTransaction?.Event is SimpleEventModel)
                        OpenDetails(_recordService.Create(plannedTransaction.Event as SimpleEventModel, 
                            x => x.Date = plannedTransaction.TransactionDate), true);

                    // repay debt
                    if (plannedTransaction?.Event is RepayDebtEventModel)
                        OpenDetails(_recordService.Create(plannedTransaction.Event as RepayDebtEventModel, 
                            x => x.Date = plannedTransaction.TransactionDate), true);

                    // money transfer
                    if (plannedTransaction?.Event is MoneyTransferEventModel)
                        OpenDetails(_moneyTransferService.Create(plannedTransaction.Event as MoneyTransferEventModel, 
                            x => x.Date = plannedTransaction.TransactionDate), true);

                }, item => item.IsPlanned, true)
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region IDashboardItem implementation

        public void Reload()
        {
            // load today transactions
            _viewModel.Entities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(
                _service.GetPlanned(GlobalVariables.UserId, _viewModel.DateFrom, DateTime.Today, false));
        }

        public bool ContainsActual => false;

        public Action ReloadActual { get; set; }

        public FrameworkElement View => this;

        public int Order => 2;

        #endregion

        #region Event handlers
               
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Reload();
        }

        #endregion

        #region Private methods

        private void OpenDetails(RecordModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new RecordDetailsView(_recordService, model, isNew), () =>
            {
                RefreshTodayTransactions();
            });
        }

        private void OpenDetails(MoneyTransferModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new MoneyTransferDetailsView(_moneyTransferService, model, isNew, false), () =>
            {
                RefreshTodayTransactions();
            });
        }

        private void ApplyTransactions(IEnumerable<ITransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                var plannedTransaction = transaction as PlannedTransactionModel<EventModel>;

                // simple event
                if (plannedTransaction?.Event is SimpleEventModel)
                    _recordService.Add(_recordService.Create(plannedTransaction.Event as SimpleEventModel, 
                        x => x.Date = plannedTransaction.TransactionDate));

                // repay debt
                if (plannedTransaction?.Event is RepayDebtEventModel)
                    _recordService.Add(_recordService.Create(plannedTransaction.Event as RepayDebtEventModel, 
                        x => x.Date = plannedTransaction.TransactionDate));

                // money transfer
                if (plannedTransaction?.Event is MoneyTransferEventModel)
                    _moneyTransferService.Add(_moneyTransferService.Create(plannedTransaction.Event as MoneyTransferEventModel,
                        x => x.Date = plannedTransaction.TransactionDate));

                RefreshTodayTransactions();
            }
        }

        private void RefreshTodayTransactions()
        {
            ReloadActual?.Invoke();
        }

        #endregion
    }
}
