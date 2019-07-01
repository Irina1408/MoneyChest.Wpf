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
        private ITransactionsSettingsService _settingsService;
        private IStorageService _storageService;
        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;
        private ICategoryService _categoryService;
        private List<StorageModel> _storages;

        #endregion

        #region Initialization

        public TransactionsPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<TransactionService>();
            _settingsService = ServiceManager.ConfigureService<TransactionsSettingsService>();
            _storageService = ServiceManager.ConfigureService<StorageService>();
            _recordService = ServiceManager.ConfigureService<RecordService>();
            _moneyTransferService = ServiceManager.ConfigureService<MoneyTransferService>();
            _categoryService = ServiceManager.ConfigureService<CategoryService>();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new TransactionsPageViewModel()
            {
                AddRecordCommand = new Command(() => 
                    OpenDetails(_recordService.PrepareNew(new RecordModel() { UserId = GlobalVariables.UserId }), true)),

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
                }, item => !item.IsPlanned, true),

                DeleteCommand = new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
                (items) => EntityViewHelper.ConfirmAndRemove(items, _service.Delete, "Transaction", items.Select(_ => _.Description), () =>
                {
                    // remove in grid
                    foreach (var item in items.ToList())
                        _viewModel.Entities.Remove(item);
                    NotifyDataChanged();
                }), 
                (items) => !items.Any(_ => _.IsPlanned)),

                ApplyNowCommand = new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
                (items) =>
                {
                    foreach(var item in items)
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

                        AddNew(newTransaction);
                    }

                    NotifyDataChanged();
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

                }, item => item.IsPlanned, true),

                DuplicateAndApplyNowCommand = new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    if (item is RecordModel)
                    {
                        var record = item as RecordModel;
                        AddNew(_recordService.Add(new RecordModel()
                        {
                            Date = DateTime.Now,
                            Description = record.Description,
                            RecordType = record.RecordType,
                            Value = record.Value,
                            Remark = record.Remark,
                            CurrencyExchangeRate = record.CurrencyExchangeRate,
                            Commission = record.Commission,
                            CommissionType = record.CommissionType,
                            CategoryId = record?.CategoryId,
                            CurrencyId = record.CurrencyId,
                            StorageId = record.StorageId,
                            DebtId = record?.DebtId,
                            UserId = record.UserId
                        }));
                    }
                    if (item is MoneyTransferModel)
                    {
                        var moneyTransfer = item as MoneyTransferModel;
                        AddNew(_moneyTransferService.Add(new MoneyTransferModel()
                        {
                            Date = DateTime.Now,
                            CurrencyExchangeRate = moneyTransfer.CurrencyExchangeRate,
                            Value = moneyTransfer.Value,
                            Description = moneyTransfer.Description,
                            Commission = moneyTransfer.Commission,
                            CommissionType = moneyTransfer.CommissionType,
                            TakeCommissionFromReceiver = moneyTransfer.TakeCommissionFromReceiver,
                            Remark = moneyTransfer.Remark,
                            StorageFromId = moneyTransfer.StorageFromId,
                            StorageToId = moneyTransfer.StorageToId,
                            CategoryId = moneyTransfer.CategoryId
                        }));
                    }

                }, item => !item.IsPlanned),

                DuplicateCommand = new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    if (item is RecordModel)
                    {
                        var record = item as RecordModel;
                        OpenDetails(new RecordModel()
                        {
                            Date = DateTime.Now,
                            Description = record.Description,
                            RecordType = record.RecordType,
                            Value = record.Value,
                            Remark = record.Remark,
                            CurrencyExchangeRate = record.CurrencyExchangeRate,
                            Commission = record.Commission,
                            CommissionType = record.CommissionType,
                            CategoryId = record?.CategoryId,
                            CurrencyId = record.CurrencyId,
                            Currency = record.Currency,
                            StorageId = record.StorageId,
                            DebtId = record?.DebtId,
                            UserId = record.UserId
                        }, true);
                    }
                    if (item is MoneyTransferModel)
                    {
                        var moneyTransfer = item as MoneyTransferModel;
                        OpenDetails(new MoneyTransferModel()
                        {
                            Date = DateTime.Now,
                            CurrencyExchangeRate = moneyTransfer.CurrencyExchangeRate,
                            Value = moneyTransfer.Value,
                            Description = moneyTransfer.Description,
                            Commission = moneyTransfer.Commission,
                            CommissionType = moneyTransfer.CommissionType,
                            TakeCommissionFromReceiver = moneyTransfer.TakeCommissionFromReceiver,
                            Remark = moneyTransfer.Remark,
                            StorageFromId = moneyTransfer.StorageFromId,
                            StorageToId = moneyTransfer.StorageToId,
                            CategoryId = moneyTransfer.CategoryId
                        }, true);
                    }

                }, item => !item.IsPlanned)
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            // load storages
            _storages = _storageService.GetListForUser(GlobalVariables.UserId);

            // load categories
            _viewModel.Categories = TreeHelper.BuildTree(_categoryService.GetActive(GlobalVariables.UserId)
                .OrderByDescending(_ => _.RecordType)
                .ThenBy(_ => _.Name)
                .ToList(), true);

            // load settings from DB 
            if (_viewModel.PeriodFilter == null || _viewModel.DataFilter == null)
            {
                var settings = _settingsService.GetForUser(GlobalVariables.UserId);

                _viewModel.PeriodFilter = settings.PeriodFilter;
                _viewModel.PeriodFilter.OnPeriodChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(settings);
                    // reload page
                    ReloadTransactions();
                };

                _viewModel.DataFilter = settings.DataFilter;
                _viewModel.DataFilter.OnFilterChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(settings);
                    // apply filter
                    ApplyDataFilter();
                };
            }

            ReloadTransactions();
        }

        private void ReloadTransactions()
        {
            // load transactions
            _viewModel.Entities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(_service.Get(GlobalVariables.UserId, _viewModel.PeriodFilter.DateFrom, _viewModel.PeriodFilter.DateUntil));

            _viewModel.Entities.CollectionChanged += (sender, e) => ApplyDataFilter();

            // apply filter now
            ApplyDataFilter();
        }

        #endregion

        #region Private methods

        private void OpenDetails(RecordModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new RecordDetailsView(_recordService, model, isNew), () =>
                {
                    // update grid
                    if (isNew) AddNew(model);
                    else UpdatePlacement(model);

                    NotifyDataChanged();
                });
        }

        private void OpenDetails(MoneyTransferModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new MoneyTransferDetailsView(_moneyTransferService, model, isNew, false,
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

        private void ApplyDataFilter() => _viewModel.FilteredEntities = _viewModel.DataFilter.ApplyFilter(_viewModel.Entities);

        #endregion
    }
}
