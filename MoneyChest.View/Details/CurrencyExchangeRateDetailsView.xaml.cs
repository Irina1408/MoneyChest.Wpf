using MoneyChest.Model.Extensions;
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
    public abstract class CurrencyExchangeRateDetailsViewBase : EntityDetailsViewBase<CurrencyExchangeRateModel, CurrencyExchangeRateModel, ICurrencyExchangeRateService>
    {
        public CurrencyExchangeRateDetailsViewBase() : base()
        { }

        public CurrencyExchangeRateDetailsViewBase(ICurrencyExchangeRateService service, CurrencyExchangeRateModel entity, bool isNew)
            : base(service, entity, isNew)
        { }
    }
    /// <summary>
    /// Interaction logic for CurrencyExchangeRateDetailsView.xaml
    /// </summary>
    public partial class CurrencyExchangeRateDetailsView : CurrencyExchangeRateDetailsViewBase
    {
        #region Private fields

        private IEnumerable<CurrencyExchangeRateModel> _existingCurrencyExchangeRates;
        private IEnumerable<CurrencyModel> _activeCurrencies;

        #endregion

        #region Initialization

        public CurrencyExchangeRateDetailsView(ICurrencyExchangeRateService service, CurrencyExchangeRateModel entity, bool isNew,
            IEnumerable<CurrencyModel> currencies, IEnumerable<CurrencyExchangeRateModel> currencyExchangeRates)
            : base(service, entity, isNew)
        {
            InitializeComponent();

            // init
            _existingCurrencyExchangeRates = currencyExchangeRates;

            // initialize datacontexts
            _activeCurrencies = currencies.Where(_ => _.IsActive || _.Id == entity.CurrencyFromId || _.Id == entity.CurrencyToId);

            comboFromCurrencies.ItemsSource = _activeCurrencies;
            comboToCurrencies.ItemsSource = _activeCurrencies;

            // disable currency comboboxes for not new currency exchange rates
            comboFromCurrencies.IsEnabled = IsNew;
            comboToCurrencies.IsEnabled = IsNew;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        #endregion

        #region Overrides

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 280;
            window.Width = 500;
        }

        protected override void SaveChanges()
        {
            if (IsNew)
            {
                var existingCurrencyExchangeRate = _existingCurrencyExchangeRates.FirstOrDefault(_ => 
                _.CurrencyFromId == WrappedEntity.Entity.CurrencyFromId && _.CurrencyToId == WrappedEntity.Entity.CurrencyToId);

                if (existingCurrencyExchangeRate != null)
                {
                    existingCurrencyExchangeRate.Rate = WrappedEntity.Entity.Rate;
                    existingCurrencyExchangeRate.SwappedCurrencies = WrappedEntity.Entity.SwappedCurrencies;

                    Service.Update(WrappedEntity.Entity);
                }
                else
                    Service.Add(WrappedEntity.Entity);
            }
            else
                Service.Update(WrappedEntity.Entity);

            WrappedEntity.IsChanged = false;
            DialogResult = true;
        }

        #endregion

        #region Event handlers

        private void comboFromCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;
            WrappedEntity.Entity.CurrencyFrom = _activeCurrencies.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.CurrencyFromId)?.ToReferenceView();
        }

        private void comboToCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;
            WrappedEntity.Entity.CurrencyTo = _activeCurrencies.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.CurrencyToId)?.ToReferenceView();
        }

        #endregion
    }
}
