using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
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
                () => OpenDetails(new CurrencyModel() { UserId = GlobalVariables.UserId }, true)),

                EditCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) => OpenDetails(item),
                (item) => true),

                DeleteCommand = new DataGridSelectedItemsCommand<CurrencyModel>(GridCurrencies,
                (items) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage(typeof(CurrencyModel), items.Select(_ => _.Name));

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation], 
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove in database
                        _service.Delete(items);
                        // remove in grid
                        foreach (var item in items.ToList())
                            _currencies.Remove(item);
                    }
                },
                (items) => !items.Any(_ => _.IsMain)),

                SetMainCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) =>
                {
                    // save main in database
                    _service.SetMain(item);
                    // refresh currency data in grid
                    UpdateMainCurrencyLocal(item);
                    // refresh commands
                    RefreshCommandsState();
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
                        if (c.IsUsed) newIndex++;
                    }

                    // update currencies in database
                    _service.Update(items);
                    RefreshCommandsState();
                },
                (items) => items.Select(e => e.IsUsed).Distinct().Count() == 1)
            };

            this.DataContext = _modelView;
            SymbolAlignmentColumn.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(CurrencySymbolAlignment));
        }

        #endregion

        #region IPage implementation

        public string Label => MultiLangResourceManager.Instance[MultiLangResourceName.Currencies];
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
                _modelView.SelectedCurrenciesAreUsed = currencies.Select(_ => _.IsUsed).First();
            }
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

        private void OpenDetails(CurrencyModel model, bool isNew = false)
        {
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new CurrencyDetailsView(_service, model, isNew, window.Close);
            // prepare window
            window.Height = 252;
            window.Width = 393;
            window.Content = detailsView;
            window.Closing += (sender, e) =>
            {
                if (!detailsView.CloseView())
                    e.Cancel = true;
            };
            // show window
            window.ShowDialog();
            if(detailsView.DialogResult)
            {
                // update grid
                if(isNew)
                {
                    // insert new currency
                    var firstNotUsed = _currencies.FirstOrDefault(_ => !_.IsUsed);
                    var newIndex = firstNotUsed != null ? _currencies.IndexOf(firstNotUsed) : _currencies.Count - 1;
                    _currencies.Insert(newIndex, model);
                }
                else
                {
                    // check current place
                    var firstNotUsed = _currencies.FirstOrDefault(_ => !_.IsUsed && _.Id != model.Id);
                    var firstNotUsedIndex = firstNotUsed != null ? _currencies.IndexOf(firstNotUsed) : _currencies.Count - 1;
                    var currenctIndex = _currencies.IndexOf(model);

                    if ((model.IsUsed && currenctIndex > firstNotUsedIndex) || (!model.IsUsed && currenctIndex < firstNotUsedIndex))
                        _currencies.Move(currenctIndex, firstNotUsedIndex);
                }

                if (model.IsMain)
                    UpdateMainCurrencyLocal(model);

                GridCurrencies.Items.Refresh();
                RefreshCommandsState();
            }
        }

        private void UpdateMainCurrencyLocal(CurrencyModel model)
        {
            foreach(var currency in _currencies)
                currency.IsMain = currency.Id == model.Id;

            // move currency in grid
            var c = _currencies.FirstOrDefault(_ => _.IsUsed == model.IsUsed);
            if (c != null)
                _currencies.Move(_currencies.IndexOf(model), _currencies.IndexOf(c));
        }

        private void RefreshCommandsState()
        {
            _modelView.EditCommand.ValidateCanExecute();
            _modelView.DeleteCommand.ValidateCanExecute();
            _modelView.SetMainCommand.ValidateCanExecute();
            _modelView.ChangeUsabilityCommand.ValidateCanExecute();

            if (GridCurrencies.SelectedItems != null && GridCurrencies.SelectedItems.Count > 0)
            {
                var currencies = GridCurrencies.SelectedItems.OfType<CurrencyModel>();
                _modelView.SelectedCurrenciesAreUsed = currencies.Select(_ => _.IsUsed).First();
            }
        }

        #endregion
    }
}
