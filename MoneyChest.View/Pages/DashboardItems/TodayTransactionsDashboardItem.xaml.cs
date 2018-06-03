using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.View.Windows;
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
    /// Interaction logic for TodayTransactionsDashboardItem.xaml
    /// </summary>
    public partial class TodayTransactionsDashboardItem : UserControl, IDashboardItem
    {
        #region Private fields

        private TodayTransactionsDashboardItemModel _viewModel;
        private ITransactionService _service;
        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;

        #endregion

        #region Initialization

        public TodayTransactionsDashboardItem()
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
            _viewModel = new TodayTransactionsDashboardItemModel()
            {
                AddRecordCommand = new Command(() =>
                    OpenDetails(_recordService.PrepareNew(new RecordModel() { UserId = GlobalVariables.UserId }), true)),

                AddMoneyTransferCommand = new Command(() => OpenDetails(new MoneyTransferModel(), true)),
                AddChequeCommand = new Command(() =>
                {
                    var chequeWindow = new ChequeWindow();
                    chequeWindow.Owner = Window.GetWindow(this);
                    if (chequeWindow.ShowDialog() == true)
                        Reload();
                }),

                EditCommand = new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    if (item is RecordModel)
                        OpenDetails(item as RecordModel);
                    if (item is MoneyTransferModel)
                        OpenDetails(item as MoneyTransferModel);
                }, item => !item.IsPlanned, true),

                DeleteCommand = new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
                (items) => EntityViewHelper.ConfirmAndRemove(items, _service.Delete, "Transaction", items.Select(_ => _.Description), () =>
                {
                    // remove in grid
                    foreach (var item in items.ToList())
                        _viewModel.Entities.Remove(item);
                }),
                (items) => !items.Any(_ => _.IsPlanned))
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region IDashboardItem implementation

        public void Reload()
        {
            // load today transactions
            _viewModel.Entities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(
                _service.Get(GlobalVariables.UserId, DateTime.Today, DateTime.Today.AddDays(1).AddMilliseconds(-1)));
        }

        public FrameworkElement View => this;

        #endregion

        #region Private methods

        private void OpenDetails(RecordModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new RecordDetailsView(_recordService, model, isNew), () =>
            {
                // update grid
                if (isNew) AddNew(model);
                else UpdatePlacement(model);

                //NotifyDataChanged();
            });
        }

        private void OpenDetails(MoneyTransferModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new MoneyTransferDetailsView(_moneyTransferService, model, isNew, false), () =>
                {
                    // update grid
                    if (isNew) AddNew(model);
                    else UpdatePlacement(model);

                    //NotifyDataChanged();
                });
        }

        private void AddNew(ITransaction transaction)
        {
            // show only today transactions
            if(transaction.TransactionDate >= DateTime.Today && transaction.TransactionDate < DateTime.Today.AddDays(1))
            {
                var lastBefore = _viewModel.Entities.LastOrDefault(x => x.TransactionDate > transaction.TransactionDate);
                if (lastBefore != null)
                    _viewModel.Entities.Insert(_viewModel.Entities.IndexOf(lastBefore) + 1, transaction);
                else
                    _viewModel.Entities.Insert(0, transaction);
            }
        }

        private void UpdatePlacement(ITransaction transaction)
        {
            // show only today transactions
            if (transaction.TransactionDate < DateTime.Today || transaction.TransactionDate >= DateTime.Today.AddDays(1))
            {
                // remove other day transaction
                _viewModel.Entities.Remove(transaction);
            }
        }

        #endregion
    }
}
