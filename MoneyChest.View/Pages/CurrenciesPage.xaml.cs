using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.ViewModel;
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
        private CurrenciesPageViewModel _viewModel;
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
            _viewModel = new CurrenciesPageViewModel()
            {
                AddCommand = new Command(
                () => OpenDetails(new CurrencyModel() { UserId = GlobalVariables.UserId }, true)),

                EditCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) => OpenDetails(item)),

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
                            _viewModel.Currencies.Remove(item);
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

                ChangeActivityCommand = new DataGridSelectedItemsCommand<CurrencyModel>(GridCurrencies,
                (items) =>
                {
                    // get new place index
                    var firstNotUsed = _viewModel.Currencies.FirstOrDefault(_ => !_.IsActive);
                    var newIndex = firstNotUsed != null ? _viewModel.Currencies.IndexOf(firstNotUsed) : _viewModel.Currencies.Count - 1;

                    // update currencies
                    foreach (var c in items)
                    {
                        c.IsActive = !c.IsActive;
                        // replace in grid
                        _viewModel.Currencies.Move(_viewModel.Currencies.IndexOf(c), newIndex);
                        if (c.IsActive) newIndex++;
                    }

                    // update currencies in database
                    _service.Update(items);
                    RefreshCommandsState();
                },
                (items) => items.Select(e => e.IsActive).Distinct().Count() == 1)
            };

            this.DataContext = _viewModel;
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
                _viewModel.SelectedCurrenciesAreActive = currencies.Select(_ => _.IsActive).First();
            }
        }

        private void GridCurrencies_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridCurrencies.SelectedItem != null)
            {
                _viewModel.EditCommand.Execute(GridCurrencies.SelectedItem);
            }
        }

        #endregion

        #region Private methods

        private void ReloadData()
        {
            // reload currencies
            _viewModel.Currencies = new ObservableCollection<CurrencyModel>(
                _service.GetListForUser(GlobalVariables.UserId)
                .OrderByDescending(_ => _.IsActive)
                .ThenByDescending(_ => _.IsMain));

            // mark as reloaded
            _reload = false;
        }

        private void OpenDetails(CurrencyModel model, bool isNew = false)
        {
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new CurrencyDetailsView(_service, model, isNew, window.Close);
            // prepare window
            window.Height = 293;
            window.Width = 400;
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
                    var firstNotUsed = _viewModel.Currencies.FirstOrDefault(_ => !_.IsActive);
                    var newIndex = firstNotUsed != null ? _viewModel.Currencies.IndexOf(firstNotUsed) : _viewModel.Currencies.Count - 1;
                    _viewModel.Currencies.Insert(newIndex, model);
                }
                else
                {
                    // check current place
                    var firstNotUsed = _viewModel.Currencies.FirstOrDefault(_ => !_.IsActive && _.Id != model.Id);
                    var firstNotUsedIndex = firstNotUsed != null ? _viewModel.Currencies.IndexOf(firstNotUsed) : _viewModel.Currencies.Count - 1;
                    var currenctIndex = _viewModel.Currencies.IndexOf(model);

                    if ((model.IsActive && currenctIndex > firstNotUsedIndex) || (!model.IsActive && currenctIndex < firstNotUsedIndex))
                        _viewModel.Currencies.Move(currenctIndex, firstNotUsedIndex);
                }

                if (model.IsMain)
                    UpdateMainCurrencyLocal(model);

                GridCurrencies.Items.Refresh();
                RefreshCommandsState();
            }
        }

        private void UpdateMainCurrencyLocal(CurrencyModel model)
        {
            foreach(var currency in _viewModel.Currencies)
                currency.IsMain = currency.Id == model.Id;

            // move currency in grid
            var c = _viewModel.Currencies.FirstOrDefault(_ => _.IsActive == model.IsActive);
            if (c != null)
                _viewModel.Currencies.Move(_viewModel.Currencies.IndexOf(model), _viewModel.Currencies.IndexOf(c));
        }

        private void RefreshCommandsState()
        {
            _viewModel.EditCommand.ValidateCanExecute();
            _viewModel.DeleteCommand.ValidateCanExecute();
            _viewModel.SetMainCommand.ValidateCanExecute();
            _viewModel.ChangeActivityCommand.ValidateCanExecute();

            if (GridCurrencies.SelectedItems != null && GridCurrencies.SelectedItems.Count > 0)
            {
                var currencies = GridCurrencies.SelectedItems.OfType<CurrencyModel>();
                _viewModel.SelectedCurrenciesAreActive = currencies.Select(_ => _.IsActive).First();
            }
        }

        #endregion
    }
}
