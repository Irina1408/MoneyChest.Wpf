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
    /// Interaction logic for AutoExecutedTransactionsDashboardItem.xaml
    /// </summary>
    public partial class AutoExecutedTransactionsDashboardItem : UserControl, IDashboardItem
    {
        #region Private fields

        private AutoExecutedTransactionsDashboardItemModel _viewModel;
        private ITransactionService _service;
        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;

        #endregion

        #region Initialization

        public AutoExecutedTransactionsDashboardItem()
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
            _viewModel = new AutoExecutedTransactionsDashboardItemModel()
            {
                DateFrom = GlobalVariables.LastUsageDate < DateTime.Today ? GlobalVariables.LastUsageDate.AddDays(1) : DateTime.Today,

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

                    ReloadActual?.Invoke();
                }),
                (items) => !items.Any(_ => _.IsPlanned))
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region IDashboardItem implementation

        public void Reload()
        {
            // load Auto executed transactions
            _viewModel.Entities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(
                _service.GetActual(GlobalVariables.UserId, _viewModel.DateFrom, DateTime.Today.AddDays(1).AddMilliseconds(-1), true));
        }

        public bool ContainsActual => false;

        public Action ReloadActual { get; set; }

        public FrameworkElement View => this;

        public int Order => 1;

        #endregion

        #region Event handlers

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            Reload();
        }

        #endregion

        #region Private methods

        private void OpenDetails(RecordModel model)
        {
            this.OpenDetailsWindow(new RecordDetailsView(_recordService, model, false), ReloadActual);
        }

        private void OpenDetails(MoneyTransferModel model)
        {
            this.OpenDetailsWindow(new MoneyTransferDetailsView(_moneyTransferService, model, false, false), ReloadActual);
        }

        #endregion
    }
}
