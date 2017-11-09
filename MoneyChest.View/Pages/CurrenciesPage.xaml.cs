using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.View.Commands;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.View.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for CurrenciesPage.xaml
    /// </summary>
    public partial class CurrenciesPage : UserControl, IPage
    {
        #region Private fields

        private ICurrencyService _service;
        private ObservableCollection<CurrencyModel> _currencies;
        private CurrenciesPageViewModel _modelView;
        // TODO: replace to IPage Options
        private bool _reload = true;

        #endregion

        #region Initialization

        public CurrenciesPage()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<CurrencyService>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _modelView = new CurrenciesPageViewModel()
            {
                AddCommand = new Command(
                () =>
                {
                    // TODO: on close window ask confirmation
                    var window = this.InitializeDependWindow(false);
                    window.Height = 252;
                    window.Width = 393;
                    window.Content = new CurrencyDetailsView(_service, new CurrencyModel()
                    {
                        UserId = GlobalVariables.UserId
                    }, true, window.Close);
                    window.ShowDialog();
                    //TODO: update grid
                    _reload = true;
                }),

                EditCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) =>
                {
                    // TODO: on close window ask confirmation
                    var window = this.InitializeDependWindow(false);
                    window.Height = 252;
                    window.Width = 393;
                    window.Content = new CurrencyDetailsView(_service, item, false, window.Close);
                    window.ShowDialog();
                    //TODO: update grid
                    _reload = true;
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
                    _service.SetMain(GlobalVariables.UserId, item.Id);

                    // refresh currency data in grid
                    foreach (var curr in _currencies.Where(_ => _.IsMain))
                        curr.IsMain = false;
                    item.IsMain = true;

                    // move currency in grid
                    var c = _currencies.FirstOrDefault(_ => _.IsUsed == item.IsUsed);
                    if (c != null)
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

            this.DataContext = _modelView;
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
                    _modelView.ChangeUsabilityVisibility = Visibility.Visible;
                    var isEnable = !currencies.First().IsUsed;
                    _modelView.EnableVisibility = isEnable ? Visibility.Visible : Visibility.Collapsed;
                    _modelView.DisableVisibility = isEnable ? Visibility.Collapsed : Visibility.Visible;
                    // TODO: language
                    _modelView.ChangeUsabilityLabel = isEnable ? "Enable" : "Disable";
                }
                else
                    _modelView.ChangeUsabilityVisibility = Visibility.Collapsed;
            }
            else
                _modelView.ChangeUsabilityVisibility = Visibility.Collapsed;
        }

        private void GridCurrencies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridCurrencies.SelectedItem != null)
            {
                _modelView.EditCommand.Execute(GridCurrencies.SelectedItem);
            }
        }

        #endregion

        #region Private methods

        private void ReloadData()
        {
            // remove handlers
            GridCurrencies.ItemsSource = null;

            // clear currenct list if it exists
            if (_currencies != null)
                _currencies.Clear();

            // load currencies
            _currencies = new ObservableCollection<CurrencyModel>(
                _service.GetListForUser(GlobalVariables.UserId)
                .OrderByDescending(_ => _.IsUsed)
                .ThenByDescending(_ => _.IsMain));

            // fill grid
            GridCurrencies.ItemsSource = _currencies;

            // mark as reloaded
            _reload = false;
        }

        #endregion
    }
}
