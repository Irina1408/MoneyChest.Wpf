using MahApps.Metro.IconPacks;
using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.View.Windows;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.Extensions;
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

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for TransactionsPage.xaml
    /// </summary>
    public partial class TransactionsPage : PageBase
    {
        #region Private fields

        private TransactionsPageViewModel _viewModel;
        private ITransactionService _service;
        private List<StorageModel> _storages;

        #endregion

        #region Initialization

        public TransactionsPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();

            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetListForUser(GlobalVariables.UserId);

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new TransactionsPageViewModel()
            {
                AddRecordCommand = new Command(() => OpenDetails(new RecordModel() { UserId = GlobalVariables.UserId }, true)),
                AddMoneyTransferCommand = new Command(() => OpenDetails(new MoneyTransferModel(), true)),
                AddChequeCommand = new Command(() =>
                {
                    var chequeWindow = new ChequeWindow();
                    chequeWindow.Owner = Window.GetWindow(this);
                    if(chequeWindow.ShowDialog() == true)
                        Reload();
                }),

                EditCommand = new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    if(item is RecordModel)
                        OpenDetails(item as RecordModel);
                    if(item is MoneyTransferModel)
                        OpenDetails(item as MoneyTransferModel);
                }, null, true),

                DeleteCommand = new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
                (items) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage("Transaction", items.Select(_ => _.Description));

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove in database
                        _service.Delete(items);
                        // remove in grid
                        foreach (var item in items.ToList())
                            _viewModel.Entities.Remove(item);
                        NotifyDataChanged();
                    }
                },
                (items) => !items.Any(_ => _.IsPlanned))
            };

            _viewModel.PrevDateRangeCommand = new Command(() => _viewModel.PeriodFilter.PrevDateRange());
            _viewModel.NextDateRangeCommand = new Command(() => _viewModel.PeriodFilter.NextDateRange());
            _viewModel.SelectDateRangeCommand = new Command(() =>
            {
                var dateFrom = _viewModel.PeriodFilter.DateFrom;
                var dateUntil = _viewModel.PeriodFilter.DateUntil;
                if (this.ShowDateRangeSelector(ref dateFrom, ref dateUntil))
                {
                    _viewModel.PeriodFilter.DateFrom = dateFrom;
                    _viewModel.PeriodFilter.DateUntil = dateUntil;
                }
            });

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            // TODO: load settings from DB 
            if (_viewModel.PeriodFilter == null)
            {
                _viewModel.PeriodFilter = new PeriodFilterModel();
                _viewModel.PeriodFilter.OnPeriodChanged += (sender, e) =>
                {
                    // reload data
                    Reload();
                };
            }

            // TODO: load filter from DB 
            if (_viewModel.DataFilter == null)
            {
                _viewModel.DataFilter = new DataFilterModel();
                _viewModel.DataFilter.PropertyChanged += (sender, e) => ApplyDataFilter();
            }

            _viewModel.Entities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(_service.Get(GlobalVariables.UserId, _viewModel.PeriodFilter.DateFrom, _viewModel.PeriodFilter.DateUntil));

            _viewModel.Entities.CollectionChanged += (sender, e) => ApplyDataFilter();

            // apply filter now
            ApplyDataFilter();
        }

        #endregion

        #region Event handlers

        //public void PeriodDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
        //{
        //    _viewModel.ViewSettings.IsDateRangeFilling = true;
        //}

        //public void PeriodDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        //{
        //    _viewModel.ViewSettings.IsDateRangeFilling = false;
        //    //periodPopup.IsPopupOpen = false;
        //    if (!Equals(eventArgs.Parameter, "1")) return;
            
        //    // reload page data
        //    Reload();
        //}

        #endregion

        #region Private methods

        private void OpenDetails(RecordModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new RecordDetailsView(model, isNew), () =>
                {
                    // update grid
                    if (isNew) AddNew(model);
                    else UpdatePlacement(model);

                    NotifyDataChanged();
                });
        }

        private void OpenDetails(MoneyTransferModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new MoneyTransferDetailsView(model, isNew, false,
                _storages.OrderByDescending(_ => _.IsVisible).ThenBy(_ => _.Name)), () =>
                {
                    // update grid
                    if (isNew) AddNew(model);
                    else UpdatePlacement(model);

                    NotifyDataChanged();
                });
        }

        private void AddNew(ITransaction transaction)
        {
            var lastBefore = _viewModel.Entities.LastOrDefault(x => x.TransactionDate > transaction.TransactionDate);
            if (lastBefore != null)
                _viewModel.Entities.Insert(_viewModel.Entities.IndexOf(lastBefore) + 1, transaction);
            else
                _viewModel.Entities.Insert(0, transaction);
        }

        private void UpdatePlacement(ITransaction transaction)
        {
            var lastBefore = _viewModel.Entities.LastOrDefault(x => x.TransactionDate > transaction.TransactionDate);
            if (lastBefore != null)
                _viewModel.Entities.Move(_viewModel.Entities.IndexOf(transaction), _viewModel.Entities.IndexOf(lastBefore) + 1);
            else
                _viewModel.Entities.Move(_viewModel.Entities.IndexOf(transaction), 0);
        }

        private void ApplyDataFilter()
        {
            Func<ITransaction, bool> filter = (t) => true;

            if (_viewModel.DataFilter.IsFilterApplied)
            {
                // TODO: replace it
                if (!string.IsNullOrEmpty(_viewModel.DataFilter.Description))
                    filter = (t) => filter(t) && !string.IsNullOrEmpty(t.Description)
                                              && t.Description.Contains(_viewModel.DataFilter.Description);

                if (!string.IsNullOrEmpty(_viewModel.DataFilter.Remark))
                    filter = (t) => filter(t) && !string.IsNullOrEmpty(t.Remark)
                                              && t.Remark.Contains(_viewModel.DataFilter.Remark);
            }

            _viewModel.FilteredEntities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(
                _viewModel.Entities.Where(filter));
        }

        #endregion
    }
}
