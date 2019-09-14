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
        private List<StorageModel> _storages;

        private ITransactionService _transactionService;
        private ITransactionsSettingsService _settingsService;
        private IStorageService _storageService;
        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;

        private ITransactionTemplateService _templatesService;
        private IRecordTemplateService _recordTemplateService;
        private IMoneyTransferTemplateService _moneyTransferTemplateService;

        #endregion

        #region Initialization

        public TransactionsPage() : base()
        {
            InitializeComponent();

            // init
            _transactionService = ServiceManager.ConfigureService<TransactionService>();
            _settingsService = ServiceManager.ConfigureService<TransactionsSettingsService>();
            _storageService = ServiceManager.ConfigureService<StorageService>();
            _recordService = ServiceManager.ConfigureService<RecordService>();
            _moneyTransferService = ServiceManager.ConfigureService<MoneyTransferService>();

            _templatesService = ServiceManager.ConfigureService<TransactionTemplateService>();
            _recordTemplateService = ServiceManager.ConfigureService<RecordTemplateService>();
            _moneyTransferTemplateService = ServiceManager.ConfigureService<MoneyTransferTemplateService>();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new TransactionsPageViewModel()
            {
                // transactions
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

                EditTransactionCommand = InitTransactionEditCommand(),
                DeleteTransactionCommand = InitTransactionDeleteCommand(),

                ApplyNowPlannedTransactionCommand = InitTransactionApplyNowCommand(),
                CreateTransactionFromPlannedCommand = InitTransactionCreateTransactionCommand(),

                DuplicateAndApplyNowTransactionCommand = InitDuplicateAndApplyNowCommand(),
                DuplicateTransactionCommand = InitDuplicateTransactionCommand(),

                CreateTemplateFromTransactionCommand = InitCreateTemplateFromTransactionCommand(),

                // templates
                AddRecordTemplateCommand = new Command(() =>
                    OpenDetails(_recordTemplateService.PrepareNew(new RecordTemplateModel() { UserId = GlobalVariables.UserId }), true)),

                AddMoneyTransferTemplateCommand = new Command(() => OpenDetails(new MoneyTransferTemplateModel(), true)),

                EditTemplateCommand = InitTemplateEditCommand(),
                DeleteTemplateCommand = InitTemplateDeleteCommand(),

                ApplyNowTemplateCommand = InitTemplateApplyNowCommand(),
                CreateTransactionFromTemplateCommand = InitTemplateCreateTransactionCommand()
            };
            
            this.DataContext = _viewModel;
        }

        #endregion

        #region Transaction Commands initialization

        private IMCCommand InitTransactionEditCommand()
        {
            return new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    // when transaction is record
                    if (item is RecordModel)
                        OpenDetails(item as RecordModel);

                    // when transaction is money transfer
                    if (item is MoneyTransferModel)
                        OpenDetails(item as MoneyTransferModel);

                }, item => !item.IsPlanned, true);
        }

        private IMCCommand InitTransactionDeleteCommand()
        {
            return new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
                (items) => EntityViewHelper.ConfirmAndRemove(items, _transactionService.Delete, "Transaction", items.Select(_ => _.Description), 
                () =>
                {
                    // remove in grid
                    foreach (var item in items.ToList())
                        _viewModel.TransactionEntities.Remove(item);
                    NotifyDataChanged();
                }),
                (items) => !items.Any(_ => _.IsPlanned));
        }

        private IMCCommand InitTransactionApplyNowCommand()
        {
            return new DataGridSelectedItemsCommand<ITransaction>(GridTransactions,
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

                        AddNew(newTransaction);
                    }

                    NotifyDataChanged();
                },
                (items) => items.All(_ => _.IsPlanned));
        }

        private IMCCommand InitTransactionCreateTransactionCommand()
        {
            return new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    var plannedTransaction = item as PlannedTransactionModel<EventModel>;

                    // simple event
                    if (plannedTransaction?.Event is SimpleEventModel)
                        OpenDetails(_recordService.Create(plannedTransaction.Event as SimpleEventModel), true, true);

                    // repay debt
                    if (plannedTransaction?.Event is RepayDebtEventModel)
                        OpenDetails(_recordService.Create(plannedTransaction.Event as RepayDebtEventModel), true, true);

                    // money transfer
                    if (plannedTransaction?.Event is MoneyTransferEventModel)
                        OpenDetails(_moneyTransferService.Create(plannedTransaction.Event as MoneyTransferEventModel), true, true);

                }, item => item.IsPlanned, true);
        }

        private IMCCommand InitDuplicateAndApplyNowCommand()
        {
            return new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    // when transaction is record
                    if (item is RecordModel)
                        AddNew(_recordService.Duplicate(item as RecordModel));

                    // when transaction is money transfer
                    if (item is MoneyTransferModel)
                        AddNew(_moneyTransferService.Duplicate(item as MoneyTransferModel));

                }, item => !item.IsPlanned);
        }

        private IMCCommand InitDuplicateTransactionCommand()
        {
            return new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    // when transaction is record
                    if (item is RecordModel)
                        OpenDetails(_recordService.Duplicate(item as RecordModel), true, true);

                    // when transaction is money transfer
                    if (item is MoneyTransferModel)
                        OpenDetails(_moneyTransferService.Duplicate(item as MoneyTransferModel), true, true);

                }, item => !item.IsPlanned);
        }

        private IMCCommand InitCreateTemplateFromTransactionCommand()
        {
            return new DataGridSelectedItemCommand<ITransaction>(GridTransactions,
                (item) =>
                {
                    // when transaction is record
                    if (item is RecordModel)
                        OpenDetails(_recordTemplateService.Create(item as RecordModel), true, true);

                    // when transaction is money transfer
                    if (item is MoneyTransferModel)
                        OpenDetails(_moneyTransferTemplateService.Create(item as MoneyTransferModel), true, true);

                }, item => !item.IsPlanned);
        }

        #endregion

        #region Templates commands initialization

        private IMCCommand InitTemplateEditCommand()
        {
            return new DataGridSelectedItemCommand<ITransactionTemplate>(GridTemplates,
                (item) =>
                {
                    // when transaction is record template
                    if (item is RecordTemplateModel)
                        OpenDetails(item as RecordTemplateModel);

                    // when transaction is money transfer
                    if (item is MoneyTransferTemplateModel)
                        OpenDetails(item as MoneyTransferTemplateModel);

                });
        }

        private IMCCommand InitTemplateDeleteCommand()
        {
            return new DataGridSelectedItemsCommand<ITransactionTemplate>(GridTemplates,
                (items) => EntityViewHelper.ConfirmAndRemove(items, _templatesService.Delete, "Template", items.Select(_ => _.Name),
                () =>
                {
                    // remove in grid
                    foreach (var item in items.ToList())
                        _viewModel.TemplateEntities.Remove(item);
                    NotifyDataChanged();
                }));
        }

        private IMCCommand InitTemplateApplyNowCommand()
        {
            return new DataGridSelectedItemsCommand<ITransactionTemplate>(GridTemplates,
                (items) =>
                {
                    foreach (var item in items)
                    {
                        ITransaction newTransaction = null;
                        var transactionTemplate = item as ITransactionTemplate;

                        // record
                        if (transactionTemplate is RecordTemplateModel)
                            newTransaction = _recordService.Add(
                                _recordTemplateService.Create(transactionTemplate as RecordTemplateModel));
                        
                        // money transfer
                        if (transactionTemplate is MoneyTransferTemplateModel)
                            newTransaction = _moneyTransferService.Add(
                                _moneyTransferTemplateService.Create(transactionTemplate as MoneyTransferTemplateModel));

                        AddNew(newTransaction);
                    }

                    NotifyDataChanged();
                });
        }

        private IMCCommand InitTemplateCreateTransactionCommand()
        {
            return new DataGridSelectedItemCommand<ITransactionTemplate>(GridTemplates,
                (item) =>
                {
                    var transactionTemplate = item as ITransactionTemplate;

                    // record
                    if (transactionTemplate is RecordTemplateModel)
                        OpenDetails(_recordTemplateService.Create(transactionTemplate as RecordTemplateModel), true, true);

                    // money transfer
                    if (transactionTemplate is MoneyTransferTemplateModel)
                        OpenDetails(_moneyTransferTemplateService.Create(transactionTemplate as MoneyTransferTemplateModel), true, true);

                }, null, true);
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            // load storages
            _storages = _storageService.GetListForUser(GlobalVariables.UserId);

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

            // load transactions list
            ReloadTransactions();

            // load templates list
            _viewModel.TemplateEntities = new System.Collections.ObjectModel.ObservableCollection<ITransactionTemplate>(
                _templatesService.Get(GlobalVariables.UserId));
        }

        private void ReloadTransactions()
        {
            // load transactions
            _viewModel.TransactionEntities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(
                _transactionService.Get(GlobalVariables.UserId, _viewModel.PeriodFilter.DateFrom, _viewModel.PeriodFilter.DateUntil));

            _viewModel.TransactionEntities.CollectionChanged += (sender, e) => ApplyDataFilter();

            // apply filter now
            ApplyDataFilter();
        }

        #endregion

        #region Details opening

        private void OpenDetails(RecordModel model, bool isNew = false, bool? allowSaveIfNoChanges = null)
        {
            this.OpenDetailsWindow(new RecordDetailsView(_recordService, model, isNew, allowSaveIfNoChanges), () =>
            {
                // update grid
                if (isNew) AddNew(model);
                else UpdatePlacement(model);

                NotifyDataChanged();
            });
        }

        private void OpenDetails(MoneyTransferModel model, bool isNew = false, bool? allowSaveIfNoChanges = null)
        {
            this.OpenDetailsWindow(new MoneyTransferDetailsView(_moneyTransferService, model, isNew, false,
                _storages.OrderByDescending(_ => _.IsVisible).ThenBy(_ => _.Name), allowSaveIfNoChanges), () =>
                {
                    // update grid
                    if (isNew) AddNew(model);
                    else UpdatePlacement(model);

                    NotifyDataChanged();
                });
        }

        private void OpenDetails(RecordTemplateModel model, bool isNew = false, bool? allowSaveIfNoChanges = null)
        {
            this.OpenDetailsWindow(new RecordTemplateDetailsView(_recordTemplateService, model, isNew, allowSaveIfNoChanges), () =>
            {
                // update grid
                if (isNew) _viewModel.TemplateEntities.Add(model);

                NotifyDataChanged();
            });
        }

        private void OpenDetails(MoneyTransferTemplateModel model, bool isNew = false, bool? allowSaveIfNoChanges = null)
        {
            this.OpenDetailsWindow(new MoneyTransferTemplateDetailsView(_moneyTransferTemplateService, model, isNew, false,
                _storages.OrderByDescending(_ => _.IsVisible).ThenBy(_ => _.Name), allowSaveIfNoChanges), () =>
                {
                    // update grid
                    if (isNew) _viewModel.TemplateEntities.Add(model);

                    NotifyDataChanged();
                });
        }

        #endregion

        #region Private methods

        private void AddNew(ITransaction transaction)
        {
            // show transaction in the view
            var lastBefore = _viewModel.TransactionEntities.LastOrDefault(x => x.TransactionDate > transaction.TransactionDate);
            if (lastBefore != null)
                _viewModel.TransactionEntities.Insert(_viewModel.TransactionEntities.IndexOf(lastBefore) + 1, transaction);
            else
                _viewModel.TransactionEntities.Insert(0, transaction);
        }

        private void UpdatePlacement(ITransaction transaction)
        {
            var lastBefore = _viewModel.TransactionEntities.LastOrDefault(x => x.TransactionDate > transaction.TransactionDate);
            if (lastBefore != null)
            {
                // adapt new index (in case when transaction should be above in list index should be increased by 1)
                var oldIndex = _viewModel.TransactionEntities.IndexOf(transaction);
                var newIndex = _viewModel.TransactionEntities.IndexOf(lastBefore);
                if (newIndex < oldIndex) newIndex++;

                _viewModel.TransactionEntities.Move(oldIndex, newIndex);
            }
            else
                _viewModel.TransactionEntities.Move(_viewModel.TransactionEntities.IndexOf(transaction), 0);
        }

        private void ApplyDataFilter() => _viewModel.TransactionFilteredEntities = _viewModel.DataFilter.ApplyFilter(_viewModel.TransactionEntities);

        #endregion
    }
}
