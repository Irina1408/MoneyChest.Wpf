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
    public partial class CurrenciesPage : PageBase
    {
        #region Private fields

        private ICurrencyService _service;
        private ICurrencyExchangeRateService _currencyExchangeRateService;
        private CurrenciesPageViewModel _viewModel;
        private bool _areCurrencyExchangeRatesLoaded = false;

        #endregion

        #region Initialization

        public CurrenciesPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<CurrencyService>();
            _currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new CurrenciesPageViewModel()
            {
                // Currency commands
                AddCurrencyCommand = new Command(
                () => OpenDetails(new CurrencyModel() { UserId = GlobalVariables.UserId }, true)),

                EditCurrencyCommand = new DataGridSelectedItemCommand<CurrencyModel>(GridCurrencies,
                (item) => OpenDetails(item), null, true),

                DeleteCurrencyCommand = new DataGridSelectedItemsCommand<CurrencyModel>(GridCurrencies,
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
                        NotifyDataChanged();
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
                    RefreshCurrencyCommandsState();
                    NotifyDataChanged();
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
                    RefreshCurrencyCommandsState();
                    NotifyDataChanged();
                },
                (items) => items.Select(e => e.IsActive).Distinct().Count() == 1),

                // Currency echange rate commands
                AddCurrencyExchangeRateCommand = new Command(
                () => OpenDetails(new CurrencyExchangeRateModel(), true)),

                EditCurrencyExchangeRateCommand = new DataGridSelectedItemCommand<CurrencyExchangeRateModel>(GridCurrencyExchangeRates,
                (item) => OpenDetails(item), doubleClick: true),

                DeleteCurrencyExchangeRateCommand = new DataGridSelectedItemsCommand<CurrencyExchangeRateModel>(GridCurrencyExchangeRates,
                (items) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage(typeof(CurrencyExchangeRateModel), 
                        items.Select(_ => $"{_.CurrencyFrom.Name} -> {_.CurrencyTo.Name}"));

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove in database
                        _currencyExchangeRateService.Delete(items);
                        // remove in grid
                        foreach (var item in items.ToList())
                            _viewModel.CurrencyExchangeRates.Remove(item);
                        NotifyDataChanged();
                    }
                })
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            // reload currencies
            _viewModel.Currencies = new ObservableCollection<CurrencyModel>(
                _service.GetListForUser(GlobalVariables.UserId)
                .OrderByDescending(_ => _.IsActive)
                .ThenByDescending(_ => _.IsMain));

            if (ExpanderCurrencyExchangeRate.IsExpanded)
                LoadCurrencyExchangeRates();
            else
                _areCurrencyExchangeRatesLoaded = false;
        }

        #endregion

        #region Event handlers

        private void GridCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GridCurrencies.SelectedItems != null && GridCurrencies.SelectedItems.Count > 0)
            {
                var currencies = GridCurrencies.SelectedItems.OfType<CurrencyModel>();
                _viewModel.SelectedCurrenciesAreActive = currencies.Select(_ => _.IsActive).First();
            }
        }

        private void ExpanderCurrencyExchangeRate_Expanded(object sender, RoutedEventArgs e)
        {
            if (!_areCurrencyExchangeRatesLoaded)
                LoadCurrencyExchangeRates();
        }

        #endregion

        #region Private methods

        private void LoadCurrencyExchangeRates()
        {
            _viewModel.CurrencyExchangeRates = new ObservableCollection<CurrencyExchangeRateModel>(
                    _currencyExchangeRateService.GetListForUser(GlobalVariables.UserId));

            _areCurrencyExchangeRatesLoaded = true;
        }

        private void OpenDetails(CurrencyModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new CurrencyDetailsView(_service, model, isNew), () =>
            {
                // update grid
                if (isNew)
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
                RefreshCurrencyCommandsState();
                NotifyDataChanged();
            });
        }
        
        private void OpenDetails(CurrencyExchangeRateModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new CurrencyExchangeRateDetailsView(_currencyExchangeRateService, model, isNew, _viewModel.Currencies, _viewModel.CurrencyExchangeRates), () =>
            {
                // update grid
                if (isNew && _viewModel.CurrencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == model.CurrencyFromId
                    && _.CurrencyToId == model.CurrencyToId) == null)
                {
                    _viewModel.CurrencyExchangeRates.Add(model);
                }

                GridCurrencyExchangeRates.Items.Refresh();
                NotifyDataChanged();
            });
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

        private void RefreshCurrencyCommandsState()
        {
            _viewModel.EditCurrencyCommand.ValidateCanExecute();
            _viewModel.DeleteCurrencyCommand.ValidateCanExecute();
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
