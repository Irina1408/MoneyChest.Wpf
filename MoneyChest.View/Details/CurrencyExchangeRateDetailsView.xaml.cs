using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Services.Services.Base;
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
    public abstract class CurrencyExchangeRateDetailsViewBase : BaseEntityDetailsView<CurrencyExchangeRateModel, CurrencyExchangeRateModel, ICurrencyExchangeRateService>
    {
        public CurrencyExchangeRateDetailsViewBase() : base()
        { }

        public CurrencyExchangeRateDetailsViewBase(ICurrencyExchangeRateService service, CurrencyExchangeRateModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }
    /// <summary>
    /// Interaction logic for CurrencyExchangeRateDetailsView.xaml
    /// </summary>
    public partial class CurrencyExchangeRateDetailsView : CurrencyExchangeRateDetailsViewBase
    {
        #region Private fields

        private IEnumerable<CurrencyExchangeRateModel> _existingCurrencyExchangeRates;

        #endregion

        #region Initialization

        public CurrencyExchangeRateDetailsView(ICurrencyExchangeRateService service, CurrencyExchangeRateModel entity, bool isNew, 
            Action closeAction, IEnumerable<CurrencyModel> currencies, IEnumerable<CurrencyExchangeRateModel> currencyExchangeRates)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();

            // init
            _existingCurrencyExchangeRates = currencyExchangeRates;

            // initialize datacontexts
            IEnumerable<CurrencyModel> activeCurrencies = 
                currencies.Where(_ => _.IsActive || _.Id == entity.CurrencyFromId || _.Id == entity.CurrencyToId);

            comboFromCurrencies.ItemsSource = activeCurrencies;
            comboToCurrencies.ItemsSource = activeCurrencies;

            // disable currency comboboxes for not new currency exchange rates
            comboFromCurrencies.IsEnabled = _isNew;
            comboToCurrencies.IsEnabled = _isNew;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Public

        public override void SaveChanges()
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

        #endregion
    }
}
