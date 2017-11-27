using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
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
    /// Interaction logic for CurrencyExchangeRateDetailsView.xaml
    /// </summary>
    public partial class CurrencyExchangeRateDetailsView : UserControl
    {
        #region Private fields

        private ICurrencyExchangeRateService _service;
        private EntityWrapper<CurrencyExchangeRateModel> _wrappedEntity;
        private bool _isNew;
        private DetailsViewCommandContainer _commands;
        private Action _closeAction;
        private bool _closeView;
        private IEnumerable<CurrencyExchangeRateModel> _existingCurrencyExchangeRates;

        #endregion

        #region Initialization

        public CurrencyExchangeRateDetailsView(ICurrencyExchangeRateService service, CurrencyExchangeRateModel entity, bool isNew, 
            Action closeAction, IEnumerable<CurrencyModel> currencies, IEnumerable<CurrencyExchangeRateModel> currencyExchangeRates)
        {
            InitializeComponent();

            // init
            _service = service;
            _isNew = isNew;
            _closeAction = closeAction;
            _existingCurrencyExchangeRates = currencyExchangeRates;

            // set defauls
            _closeView = false;
            LabelHeader.Content = isNew
                ? MultiLangResourceManager.Instance[MultiLangResourceName.New(typeof(CurrencyExchangeRateModel))]
                : MultiLangResourceManager.Instance[MultiLangResourceName.Singular(typeof(CurrencyExchangeRateModel))];
            // initialize datacontexts
            IEnumerable<CurrencyModel> activeCurrencies;

            if (isNew)
                activeCurrencies = currencies.Where(_ => _.IsActive);
            else
                activeCurrencies = currencies.Where(_ => _.IsActive || _.Id == entity.CurrencyFromId || _.Id == entity.CurrencyToId);

            comboFromCurrencies.ItemsSource = activeCurrencies;
            comboToCurrencies.ItemsSource = activeCurrencies;

            // disable currency comboboxes for not new currency exchange rates
            comboFromCurrencies.IsEnabled = _isNew;
            comboToCurrencies.IsEnabled = _isNew;

            _wrappedEntity = new EntityWrapper<CurrencyExchangeRateModel>(entity);
            this.DataContext = _wrappedEntity.Entity;
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
                () => _wrappedEntity.IsChanged && !_wrappedEntity.HasErrors 
                    && _wrappedEntity.Entity.CurrencyFromId != _wrappedEntity.Entity.CurrencyToId),

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

        #region Public

        public bool DialogResult { get; private set; } = false;

        public void SaveChanges()
        {
            if (_isNew)
            {
                var existingCurrencyExchangeRate = _existingCurrencyExchangeRates.FirstOrDefault(_ => 
                _.CurrencyFromId == _wrappedEntity.Entity.CurrencyFromId && _.CurrencyToId == _wrappedEntity.Entity.CurrencyToId);

                if (existingCurrencyExchangeRate != null)
                {
                    existingCurrencyExchangeRate.Rate = _wrappedEntity.Entity.Rate;
                    _service.Update(_wrappedEntity.Entity);
                }
                else
                    _service.Add(_wrappedEntity.Entity);
            }
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
