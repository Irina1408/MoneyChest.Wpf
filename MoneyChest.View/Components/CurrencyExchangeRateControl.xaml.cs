using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
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

namespace MoneyChest.View.Components
{
    /// <summary>
    /// Interaction logic for CurrencyExchangeRateControl.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class CurrencyExchangeRateControl : UserControl
    {
        #region Private fields

        private List<CurrencyExchangeRateModel> currencyEchangeRates;
        private List<CurrencyReference> currencies;

        #endregion

        #region Initialization

        public CurrencyExchangeRateControl()
        {
            InitializeComponent();
        }

        #endregion

        #region ShowTakeExisting Property

        public bool ShowTakeExisting
        {
            get => (bool)this.GetValue(ShowTakeExistingProperty);
            set => this.SetValue(ShowTakeExistingProperty, value);
        }

        public static readonly DependencyProperty ShowTakeExistingProperty = DependencyProperty.Register(
            nameof(ShowTakeExisting), typeof(bool), typeof(CurrencyExchangeRateControl), new PropertyMetadata(false));

        #endregion

        #region TakeExistingCurrencyExchangeRate Property

        public bool TakeExistingCurrencyExchangeRate
        {
            get => (bool)this.GetValue(TakeExistingCurrencyExchangeRateProperty);
            set => this.SetValue(TakeExistingCurrencyExchangeRateProperty, value);
        }

        public static readonly DependencyProperty TakeExistingCurrencyExchangeRateProperty = DependencyProperty.Register(
            nameof(TakeExistingCurrencyExchangeRate), typeof(bool), typeof(CurrencyExchangeRateControl), 
            new PropertyMetadata(true, TakeExistingCurrencyExchangeRateChangedCallback));

        private static void TakeExistingCurrencyExchangeRateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get control
            var c = (d as CurrencyExchangeRateControl);
            // reload ExistingCurrencyExchangeRate
            if (c.TakeExistingCurrencyExchangeRate)
                c.UpdateCurrencyExchangeRate();
            // update possibility to enter value
            c.ExchangeRateValueGrid.IsEnabled = c.IsEnabled 
                && (c.ShowTakeExisting && !c.TakeExistingCurrencyExchangeRate || !c.ShowTakeExisting);
        }

        #endregion

        #region CurrencyFromId Property

        public int CurrencyFromId
        {
            get => (int)this.GetValue(CurrencyFromIdProperty);
            set => this.SetValue(CurrencyFromIdProperty, value);
        }

        public static readonly DependencyProperty CurrencyFromIdProperty = DependencyProperty.Register(
            nameof(CurrencyFromId), typeof(int), typeof(CurrencyExchangeRateControl),
            new FrameworkPropertyMetadata(0, SelectedCurrencyIdChangedCallback));

        #endregion

        #region CurrencyToId Property

        public int CurrencyToId
        {
            get => (int)this.GetValue(CurrencyToIdProperty);
            set => this.SetValue(CurrencyToIdProperty, value);
        }

        public static readonly DependencyProperty CurrencyToIdProperty = DependencyProperty.Register(
            nameof(CurrencyToId), typeof(int), typeof(CurrencyExchangeRateControl),
            new FrameworkPropertyMetadata(0, SelectedCurrencyIdChangedCallback));

        #endregion

        #region CurrencyIds Property

        public IEnumerable<int> CurrencyIds
        {
            get => (IEnumerable<int>)this.GetValue(CurrencyIdsProperty);
            set => this.SetValue(CurrencyIdsProperty, value);
        }

        public static readonly DependencyProperty CurrencyIdsProperty = DependencyProperty.Register(
            nameof(CurrencyIds), typeof(IEnumerable<int>), typeof(CurrencyExchangeRateControl),
            new FrameworkPropertyMetadata(null, CurrencyIdsChangedCallback));

        private static void CurrencyIdsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get control
            var c = (d as CurrencyExchangeRateControl);
            // reload currencies
            c.currencies = c.LoadCurrencies();
            // reload currency exchange rates
            if (c.currencyEchangeRates != null)
                c.currencyEchangeRates = c.LoadCurrencyEchangeRates();
        }

        #endregion

        #region CurrencyExchangeRate Property

        public decimal CurrencyExchangeRate
        {
            get => (decimal)this.GetValue(CurrencyExchangeRateProperty);
            set => this.SetValue(CurrencyExchangeRateProperty, value);
        }

        public static readonly DependencyProperty CurrencyExchangeRateProperty = DependencyProperty.Register(
            nameof(CurrencyExchangeRate), typeof(decimal), typeof(CurrencyExchangeRateControl),
            new FrameworkPropertyMetadata((decimal)1, CurrencyExchangeRateChangedCallback));

        private static void CurrencyExchangeRateChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get control
            var c = (d as CurrencyExchangeRateControl);
            // update example
            c.txtExchangeRateExample.Text = c.ExchangeRateExample;
        }

        #endregion

        #region Private properies
        
        private List<CurrencyReference> Currencies => currencies ?? (currencies = LoadCurrencies());
        private List<CurrencyExchangeRateModel> CurrencyEchangeRates => 
            currencyEchangeRates ?? (currencyEchangeRates = LoadCurrencyEchangeRates());

        private CurrencyReference CurrencyFrom => CurrencyFromId > 0 ? Currencies.FirstOrDefault(x => x.Id == CurrencyFromId) : null;
        private CurrencyReference CurrencyTo => CurrencyToId > 0 ? Currencies.FirstOrDefault(x => x.Id == CurrencyToId) : null;

        private string ExchangeRateExample => CurrencyFrom != null && CurrencyTo != null
            ? $"{CurrencyFrom.FormatValue(1)} = {CurrencyTo.FormatValue(CurrencyExchangeRate)}"
            : null;

        #endregion

        #region Private methods & properies

        private List<CurrencyExchangeRateModel> LoadCurrencyEchangeRates()
        {
            var currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();

            return CurrencyIds != null 
                ? currencyExchangeRateService.GetList(CurrencyIds.ToList())
                : currencyExchangeRateService.GetListForUser(GlobalVariables.UserId);
        }

        private List<CurrencyReference> LoadCurrencies()
        {
            var currencyService = ServiceManager.ConfigureService<CurrencyService>();

            return currencyService.GetActive(GlobalVariables.UserId, CurrencyIds?.Select(x => (int?)x)?.ToArray())
                .ConvertAll(x => x.ToReferenceView());
        }

        private void UpdateCurrencyExchangeRate()
        {
            // update CurrencyExchangeRate
            CurrencyExchangeRate = CurrencyFromId != CurrencyToId && CurrencyFromId > 0 && CurrencyToId > 0
                ? CurrencyEchangeRates.FirstOrDefault(x => x.CurrencyFromId == CurrencyFromId && x.CurrencyToId == CurrencyToId)?.Rate ?? 1
                : 1;

            // update example
            txtExchangeRateExample.Text = ExchangeRateExample;
        }

        private static void SelectedCurrencyIdChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get control
            var c = (d as CurrencyExchangeRateControl);
            c.IsEnabled = c.CurrencyFromId > 0 && c.CurrencyToId > 0 && c.CurrencyFromId != c.CurrencyToId;
            // update possibility to enter value
            c.ExchangeRateValueGrid.IsEnabled = c.IsEnabled
                && (c.ShowTakeExisting && !c.TakeExistingCurrencyExchangeRate || !c.ShowTakeExisting);
            // update currency exchange rate
            c.UpdateCurrencyExchangeRate();
        }

        #endregion
    }
}
