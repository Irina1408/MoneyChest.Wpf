using MahApps.Metro.IconPacks;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.View.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for CurrenciesPage.xaml
    /// </summary>
    public partial class CurrenciesPage : UserControl, IPage
    {
        #region Private fields

        private ICurrencyService _currencyService;
        private ObservableCollection<CurrencyModel> _currencies;
        private CurrenciesPageModelView _currenciesPageModelView;
        // TODO: replace to IPage Options
        private bool _reload = true;

        #endregion

        #region Initialization

        public CurrenciesPage()
        {
            InitializeComponent();

            // init
            _currencyService = ServiceManager.ConfigureService<CurrencyService>();
            InitializeViewModel();
        }

        #endregion

        #region IPage implementation

        public string Label => "Currencies";
        public FrameworkElement Icon { get; private set; } = new PackIconModern() { Kind = PackIconModernKind.CurrencyDollar };
        public int Order => 8;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion

        #region Event handlers

        private void CurrenciesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_reload)
                ReloadData();
        }

        private void GridCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridCurrencies.SelectedItems != null && GridCurrencies.SelectedItems.Count > 0)
            {
                var currencies = GridCurrencies.SelectedItems.OfType<CurrencyModel>();
                if (currencies.Select(x => x.IsUsed).Distinct().Count() == 1)
                {
                    _currenciesPageModelView.ChangeUsabilityVisibility = Visibility.Visible;
                    // TODO: language
                    var isEnable = !currencies.First().IsUsed;
                    _currenciesPageModelView.EnableVisibility = isEnable ? Visibility.Visible : Visibility.Collapsed;
                    _currenciesPageModelView.DisableVisibility = isEnable ? Visibility.Collapsed : Visibility.Visible;
                    _currenciesPageModelView.ChangeUsabilityLabel = isEnable ? "Enable" : "Disable";
                }
                else
                    _currenciesPageModelView.ChangeUsabilityVisibility = Visibility.Collapsed;
            }
            else
                _currenciesPageModelView.ChangeUsabilityVisibility = Visibility.Collapsed;
        }

        #endregion

        #region Private methods

        private void InitializeViewModel()
        {
            _currenciesPageModelView = new CurrenciesPageModelView()
            {
                AddCommand = new Command(
                () =>
                {
                    // TODO: open details to add new item
                }),

                EditCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) =>
                {
                    // TODO: open details to edit item
                }),

                DeleteCommand = new DataGridSelectedItemsCommand<CurrencyModel>(GridCurrencies,
                (items) =>
                {
                    // TODO: confirm before delete
                    // TODO: remove all items from database
                    // TODO: remove items from grid
                },
                (items) => items.Count() != _currencies.Count),

                SetMainCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) =>
                {
                    // save main in database
                    _currencyService.SetMain(GlobalVariables.UserId, item.Id);

                    // refresh currency data in grid
                    foreach(var curr in _currencies.Where(_ => _.IsMain))
                        curr.IsMain = false;
                    item.IsMain = true;

                    // move currency in grid
                    var c = _currencies.FirstOrDefault(_ => _.IsUsed == item.IsUsed);
                    if(c != null)
                        _currencies.Move(_currencies.IndexOf(item), _currencies.IndexOf(c));
                }, 
                (item) => !item.IsMain),

                ChangeUsabilityCommand = new DataGridSelectedItemsCommand<CurrencyModel>(GridCurrencies,
                (items) =>
                {
                    // get new place index
                    var firstNotUsed = _currencies.FirstOrDefault(_ => !_.IsUsed);
                    var newIndex = firstNotUsed != null ? _currencies.IndexOf(firstNotUsed) : _currencies.Count - 1;

                    // update currencies
                    foreach (var c in items)
                    {
                        c.IsUsed = !c.IsUsed;
                        // replace in grid
                        _currencies.Move(_currencies.IndexOf(c), newIndex);
                    }

                    // TODO: update currencies in database

                    // clear grid selection
                    GridCurrencies.SelectedItems.Clear();
                },
                (items) => items.Select(e => e.IsUsed).Distinct().Count() == 1)
            };

            this.DataContext = _currenciesPageModelView;
        }

        private void ReloadData()
        {
            // remove handlers
            GridCurrencies.ItemsSource = null;

            // clear currenct list if it exists
            if (_currencies != null)
                _currencies.Clear();

            // load currencies
            _currencies = new ObservableCollection<CurrencyModel>(
                _currencyService.GetListForUser(GlobalVariables.UserId)
                .OrderByDescending(_ => _.IsUsed)
                .ThenByDescending(_ => _.IsMain));

            // fill grid
            GridCurrencies.ItemsSource = _currencies;

            // mark as reloaded
            _reload = false;
        }

        #endregion
    }

    public class CurrenciesPageModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand SetMainCommand { get; set; }
        public ICommand ChangeUsabilityCommand { get; set; }

        // TODO: ChangeUsabilityLabel and ChangeUsabilityVisibility can be replaced to special command
        public string ChangeUsabilityLabel { get; set; }
        public Visibility EnableVisibility { get; set; }
        public Visibility DisableVisibility { get; set; } = Visibility.Collapsed;
        public Visibility ChangeUsabilityVisibility { get; set; }
    }
}
