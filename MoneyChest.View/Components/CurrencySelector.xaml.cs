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
    /// Interaction logic for CurrencySelector.xaml
    /// </summary>
    public partial class CurrencySelector : UserControl
    {
        public CurrencySelector()
        {
            InitializeComponent();
        }

        #region CurrencyId Property

        public int CurrencyId
        {
            get => (int)this.GetValue(CurrencyIdProperty);
            set => this.SetValue(CurrencyIdProperty, value);
        }

        public static readonly DependencyProperty CurrencyIdProperty = DependencyProperty.Register(
            nameof(CurrencyId), typeof(int), typeof(CurrencySelector), new PropertyMetadata(0, CurrencyIdChangedCallback));

        private static void CurrencyIdChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var currencySelector = (d as CurrencySelector);
            currencySelector.CurrencyReference = currencySelector.Currencies
                .FirstOrDefault(x => x.Id == currencySelector.CurrencyId)?.ToReferenceView();
        }

        #endregion

        #region CurrencyReference Property

        public CurrencyReference CurrencyReference
        {
            get => (CurrencyReference)this.GetValue(CurrencyReferenceProperty);
            set => this.SetValue(CurrencyReferenceProperty, value);
        }

        public static readonly DependencyProperty CurrencyReferenceProperty = DependencyProperty.Register(
            nameof(CurrencyReference), typeof(CurrencyReference), typeof(CurrencySelector));

        #endregion

        #region Currencies Property

        public IEnumerable<CurrencyModel> Currencies
        {
            get
            {
                if((IEnumerable<CurrencyModel>)this.GetValue(CurrenciesProperty) == null)
                {
                    ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
                    var currencies = currencyService.GetActive(GlobalVariables.UserId, CurrencyId);
                    this.SetValue(CurrenciesProperty, currencies);
                }

                return (IEnumerable<CurrencyModel>)this.GetValue(CurrenciesProperty);
            }
            set => this.SetValue(CurrenciesProperty, value);
        }

        public static readonly DependencyProperty CurrenciesProperty = DependencyProperty.Register(
            nameof(Currencies), typeof(IEnumerable<CurrencyModel>), typeof(CurrencySelector));

        #endregion
    }
}
