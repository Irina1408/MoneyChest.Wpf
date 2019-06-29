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
    /// Interaction logic for PlannedForTodayTransactionsDashboardItem.xaml
    /// </summary>
    public partial class PlannedForTodayTransactionsDashboardItem : UserControl, IDashboardItem
    {
        #region Private fields

        private PlannedForTodayTransactionsDashboardItemModel _viewModel;
        private ITransactionService _service;
        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;

        #endregion

        #region Initialization

        public PlannedForTodayTransactionsDashboardItem()
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
            _viewModel = new PlannedForTodayTransactionsDashboardItemModel()
            {
                ApplyNowCommand = new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
                (items) =>
                {
                    foreach (var item in items)
                    {
                        ITransaction newTransaction = null;
                        var plannedTransaction = item as PlannedTransactionModel<EventModel>;

                        // simple event
                        if (plannedTransaction?.Event is SimpleEventModel)
                            newTransaction = _recordService.Add(
                                _recordService.Create(plannedTransaction.Event as SimpleEventModel));

                        // repay debt
                        if (plannedTransaction?.Event is RepayDebtEventModel)
                            newTransaction = _recordService.Add(
                                _recordService.Create(plannedTransaction.Event as RepayDebtEventModel));

                        // money transfer
                        if (plannedTransaction?.Event is MoneyTransferEventModel)
                            newTransaction = _moneyTransferService.Add(
                                _moneyTransferService.Create(plannedTransaction.Event as MoneyTransferEventModel));

                        RefreshTodayTransactions();
                    }
                },
                (items) => items.All(_ => _.IsPlanned)),

                CreateTransactionCommand = new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    var plannedTransaction = item as PlannedTransactionModel<EventModel>;

                    // simple event
                    if (plannedTransaction?.Event is SimpleEventModel)
                        OpenDetails(_recordService.Create(plannedTransaction.Event as SimpleEventModel), true);

                    // repay debt
                    if (plannedTransaction?.Event is RepayDebtEventModel)
                        OpenDetails(_recordService.Create(plannedTransaction.Event as RepayDebtEventModel), true);

                    // money transfer
                    if (plannedTransaction?.Event is MoneyTransferEventModel)
                        OpenDetails(_moneyTransferService.Create(plannedTransaction.Event as MoneyTransferEventModel), true);

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
                _service.GetPlanned(GlobalVariables.UserId, DateTime.Today, DateTime.Today, true));
        }

        public FrameworkElement View => this;

        public int Order => 1;

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

        private void RefreshTodayTransactions()
        {
            // TODO: refresh today transactions list
        }

        #endregion
    }
}
