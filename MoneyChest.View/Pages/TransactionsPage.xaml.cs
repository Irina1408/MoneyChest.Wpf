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
                    Reload();
                };

                _viewModel.DataFilter = settings.DataFilter;
                _viewModel.DataFilter.PropertyChanged += (sender, e) =>
                {
                    // save changes
                    _settingsService.Update(settings);
                    // apply filter
                    ApplyDataFilter();
                };
            }

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
            var filters = new List<Func<ITransaction, bool>>();

            if (_viewModel.DataFilter.IsFilterApplied)
            {
                // TODO: replace filter builder
                if (!string.IsNullOrEmpty(_viewModel.DataFilter.Description))
                    filters.Add((t) => !string.IsNullOrEmpty(t.Description) && t.Description.Contains(_viewModel.DataFilter.Description));

                if (!string.IsNullOrEmpty(_viewModel.DataFilter.Remark))
                    filters.Add((t) => !string.IsNullOrEmpty(t.Remark) && t.Remark.Contains(_viewModel.DataFilter.Remark));

                if (_viewModel.DataFilter.TransactionType.HasValue)
                    filters.Add((t) => t.TransactionType == _viewModel.DataFilter.TransactionType.Value);

                if (_viewModel.DataFilter.CategoryIds.Count > 0)
                    filters.Add((t) => (_viewModel.DataFilter.CategoryIds.Contains(-1) && t.TransactionCategory == null)
                        || (t.TransactionCategory != null && _viewModel.DataFilter.CategoryIds.Contains(t.TransactionCategory.Id)));

                if (_viewModel.DataFilter.StorageIds.Count > 0)
                    filters.Add((t) => t.TransactionStorageIds.Any(x => _viewModel.DataFilter.StorageIds.Contains(x)));
            }

            _viewModel.FilteredEntities = new System.Collections.ObjectModel.ObservableCollection<ITransaction>(
                _viewModel.Entities.Where(x => filters.Count == 0 || filters.All(f => f(x))));
        }

        #endregion
    }
}
