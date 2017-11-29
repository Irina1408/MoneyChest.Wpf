using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using MoneyChest.ViewModel.Wrappers;
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

namespace MoneyChest.View.Details
{
    /// <summary>
    /// Interaction logic for MoneyTransferDetailsView.xaml
    /// </summary>
    public partial class MoneyTransferDetailsView : UserControl
    {
        #region Private fields

        private IMoneyTransferService _service;
        private EntityWrapper<MoneyTransferModel> _wrappedEntity;
        private bool _isNew;
        private DetailsViewCommandContainer _commands;
        private Action _closeAction;
        private bool _closeView;

        private IEnumerable<StorageModel> _storages;
        private IEnumerable<CurrencyExchangeRateModel> _currencyExchangeRates;
        private CategoryViewModelCollection _categories;
        private ICurrencyExchangeRateService _currencyExchangeRateService;
        private bool _showHiddenStorages;

        #endregion

        #region Initialization

        public MoneyTransferDetailsView(IMoneyTransferService service, MoneyTransferModel entity, bool isNew, Action closeAction,
            bool showHiddenStorages, IEnumerable<StorageModel> storages)
        {
            InitializeComponent();

            // init
            _service = service;
            _isNew = isNew;
            _closeAction = closeAction;
            _showHiddenStorages = showHiddenStorages;
            _storages = storages;
            _currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();

            // load categories
            ICategoryService categoryService = ServiceManager.ConfigureService<CategoryService>();
            _categories = TreeHelper.BuildTree(categoryService.GetActiveCategories(GlobalVariables.UserId)
                .OrderByDescending(_ => _.TransactionType)
                .ThenBy(_ => _.Name)
                .ToList(), entity.CategoryId, true);

            // update selected category name
            var selectedCategory = _categories.GetDescendants().FirstOrDefault(_ => _.IsSelected);
            txtCategory.Text = selectedCategory.Name;

            // set defauls
            _closeView = false;
            LabelHeader.Content = isNew
                ? MultiLangResourceManager.Instance[MultiLangResourceName.New(typeof(MoneyTransferModel))]
                : MultiLangResourceManager.Instance[MultiLangResourceName.Singular(typeof(MoneyTransferModel))];

            // initialize datacontexts
            IEnumerable<StorageModel> showStorages;
            if (_showHiddenStorages)
                showStorages = storages;
            else
                showStorages = storages.Where(_ => _.IsVisible || entity.StorageFromId == _.Id || entity.StorageToId == _.Id);

            comboFromStorage.ItemsSource = showStorages;
            comboToStorage.ItemsSource = showStorages;

            _wrappedEntity = new EntityWrapper<MoneyTransferModel>(entity);
            this.DataContext = _wrappedEntity.Entity;
            TreeViewCategories.ItemsSource = _categories;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _commands = new DetailsViewCommandContainer()
            {
                SaveCommand = new Command(() =>
                {
                    // save changes
                    SaveChanges();
                    // close control
                    _closeAction?.Invoke();
                },
                () => _wrappedEntity.IsChanged && !_wrappedEntity.HasErrors),

                CancelCommand = new Command(() =>
                {
                    if (CloseView())
                        _closeAction?.Invoke();
                })
            };

            // add events
            _wrappedEntity.Entity.PropertyChanged += (sender, args) => ((Command)_commands.SaveCommand).ValidateCanExecute();
            // validate save command now 
            _commands.SaveCommand.ValidateCanExecute();

            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers

        public void CategoryDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;

            var selectedCategory = _categories.GetDescendants().FirstOrDefault(_ => _.IsSelected);
            if (selectedCategory == null)
            {
                eventArgs.Cancel();
                return;
            }

            _wrappedEntity.Entity.CategoryId = selectedCategory.Id != -1 ? (int?)selectedCategory.Id : null;
            txtCategory.Text = selectedCategory.Name;
        }

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            var oldCurrencyFrom = _wrappedEntity.Entity.StorageFromCurrency?.Id;
            var oldCurrencyTo = _wrappedEntity.Entity.StorageToCurrency?.Id;

            // update storages currency
            _wrappedEntity.Entity.StorageFromCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageFromId)?.Currency;

            _wrappedEntity.Entity.StorageToCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageToId)?.Currency;

            if (_wrappedEntity.Entity.StorageFromCurrency != null && _wrappedEntity.Entity.StorageToCurrency != null &&
                _wrappedEntity.Entity.StorageFromCurrency.Id != _wrappedEntity.Entity.StorageToCurrency.Id)
            {
                // not update currency exchange rate if currencies was not changed
                if (oldCurrencyFrom != null && oldCurrencyTo != null && oldCurrencyFrom.Value == _wrappedEntity.Entity.StorageFromCurrency.Id && oldCurrencyTo.Value == _wrappedEntity.Entity.StorageToCurrency.Id)
                    return;

                // load _currencyExchangeRates and set correspond rate
                if (_currencyExchangeRates == null)
                {
                    _currencyExchangeRates =
                        _currencyExchangeRateService.GetList(_storages.Select(_ => _.CurrencyId).Distinct().ToList());
                }

                _wrappedEntity.Entity.CurrencyExchangeRate =
                    _currencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == _wrappedEntity.Entity.StorageFromCurrency.Id &&
                        _.CurrencyToId == _wrappedEntity.Entity.StorageToCurrency.Id)?.Rate ?? 1;
            }
            else
                _wrappedEntity.Entity.CurrencyExchangeRate = 1;
        }

        #endregion

        #region Public

        public bool DialogResult { get; private set; } = false;

        public void SaveChanges()
        {
            if (_isNew)
                _service.Add(_wrappedEntity.Entity);
            else
                _service.Update(_wrappedEntity.Entity);

            DialogResult = true;
            _closeView = true;
        }

        public void RevertChanges()
        {
            _wrappedEntity.RevertChanges();

            DialogResult = false;
            _closeView = true;
        }

        public bool CloseView()
        {
            // not ask confirmation if it has already asked
            if (_closeView) return _closeView;

            // ask confirmation only if any changes exists
            if (_wrappedEntity.IsChanged)
            {
                // show confirmation
                var dialogResult = MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmationMessage], MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmation], MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation, MessageBoxResult.Yes);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    // check errors
                    if (_wrappedEntity.HasErrors)
                        MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailedMessage], MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailed], MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    else
                        SaveChanges();
                }
                else if (dialogResult == MessageBoxResult.No)
                    RevertChanges();
            }
            else
                _closeView = true;

            return _closeView;
        }

        #endregion
    }
}
