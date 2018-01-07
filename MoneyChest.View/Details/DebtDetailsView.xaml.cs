﻿using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
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
    /// Interaction logic for DebtDetailsView.xaml
    /// </summary>
    public partial class DebtDetailsView : UserControl
    {
        #region Private fields

        private IDebtService _service;
        private EntityWrapper<DebtViewModel> _wrappedEntity;
        private bool _isNew;
        private DetailsViewCommandContainer _commands;
        private Action _closeAction;
        private bool _closeView;

        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;
        private IEnumerable<CurrencyExchangeRateModel> _currencyExchangeRates;
        private CategoryViewModelCollection _categories;
        private ICommand DeletePenaltyCommand;

        private Dictionary<DebtPenaltyViewModel, ContentControl> _penaltyControl;

        #endregion

        #region Initialization

        public DebtDetailsView(IDebtService service, DebtViewModel entity, bool isNew, Action closeAction)
        {
            InitializeComponent();

            // init
            _service = service;
            _isNew = isNew;
            _closeAction = closeAction;

            // load categories
            ICategoryService categoryService = ServiceManager.ConfigureService<CategoryService>();
            _categories = TreeHelper.BuildTree(categoryService.GetActive(GlobalVariables.UserId, entity.CategoryId)
                .OrderByDescending(_ => _.TransactionType)
                .ThenBy(_ => _.Name)
                .ToList(), entity.CategoryId);

            // update selected category name
            var selectedCategory = _categories.GetDescendants().FirstOrDefault(_ => _.IsSelected);
            txtCategory.Text = selectedCategory.Name;

            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId);
            // add empty storage
            var storages = new List<StorageModel>();
            storages.Add(new StorageModel() { Id = -1, Name = MultiLangResourceManager.Instance[MultiLangResourceName.None] });
            storages.AddRange(_storages);
            comboStorage.ItemsSource = storages;
            if (_isNew)
                entity.StorageId = -1;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.CurrencyId, entity.Storage?.CurrencyId);
            comboCurrencies.ItemsSource = _currencies;

            // set defauls
            _closeView = false;
            LabelHeader.Content = isNew
                ? MultiLangResourceManager.Instance[MultiLangResourceName.New(typeof(DebtModel))]
                : MultiLangResourceManager.Instance[MultiLangResourceName.Singular(typeof(DebtModel))];

            // initialize datacontexts
            // TODO: should be wrapped the same debt model object
            _wrappedEntity = new EntityWrapper<DebtViewModel>(entity);
            TreeViewCategories.ItemsSource = _categories;
            this.DataContext = _wrappedEntity.Entity;
            InitializeCommands();
            FillPenalties();
            // mark as not changed forcibly
            _wrappedEntity.IsChanged = false;
            // validate save command now 
            _commands.SaveCommand.ValidateCanExecute();
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
                () => _wrappedEntity.IsChanged && !_wrappedEntity.HasErrors),

                CancelCommand = new Command(() =>
                {
                    if (CloseView())
                        _closeAction?.Invoke();
                })
            };

            DeletePenaltyCommand = new ParametrizedCommand<DebtPenaltyViewModel>(
                (item) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage(typeof(DebtPenaltyModel), new[] { item.Description });

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove from entity
                        _wrappedEntity.Entity.Penalties.Remove(_wrappedEntity.Entity.Penalties.First(_ => _.Id == item.Id));
                        // remove from view
                        PenaltiesPanel.Children.Remove(_penaltyControl[item]);
                    }
                });

            _wrappedEntity.Entity.AddPenaltyCommand = new Command(() =>
            {
                var newPenalty = new DebtPenaltyModel()
                {
                    DebtId = _wrappedEntity.Entity.Id,
                    Id = _wrappedEntity.Entity.Penalties.Count > 0 ?_wrappedEntity.Entity.Penalties.Min(_ => _.Id) -1 : -1
                };

                _wrappedEntity.Entity.Penalties.Add(newPenalty);
                AddPenaltyToView(newPenalty);
            });

            // add events
            _wrappedEntity.Entity.PropertyChanged += (sender, args) => _commands.SaveCommand.ValidateCanExecute();
            // validate save command now 
            _commands.SaveCommand.ValidateCanExecute();

            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers

        public void CategoryDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;

            var selectedCategory = _categories.GetDescendants().FirstOrDefault(_ => _.IsSelected);
            if (selectedCategory == null)
            {
                eventArgs.Cancel();
                return;
            }

            _wrappedEntity.Entity.CategoryId = selectedCategory.Id != -1 ? (int?)selectedCategory.Id : null;
            txtCategory.Text = selectedCategory.Name;
        }

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            var oldCurrency = _wrappedEntity.Entity.Storage?.CurrencyId;

            // update debt currency
            _wrappedEntity.Entity.StorageCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageId)?.Currency;
            
            if (_wrappedEntity.Entity.StorageCurrency != null && _wrappedEntity.Entity.StorageCurrency.Id != _wrappedEntity.Entity.CurrencyId)
            {
                // not update currency exchange rate if currencies was not changed
                if (oldCurrency != null && _wrappedEntity.Entity.StorageCurrency.Id == oldCurrency.Value)
                    return;

                // load _currencyExchangeRates and set correspond rate
                if (_currencyExchangeRates == null)
                {
                    ICurrencyExchangeRateService currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();

                    _currencyExchangeRates =
                        currencyExchangeRateService.GetList(
                            _storages.Select(_ => _.CurrencyId).Distinct().Concat(_currencies.Select(c => c.Id)).Distinct().ToList());
                }

                _wrappedEntity.Entity.CurrencyExchangeRate =
                    _currencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == _wrappedEntity.Entity.CurrencyId &&
                        _.CurrencyToId == _wrappedEntity.Entity.StorageCurrency.Id)?.Rate ?? 1;
            }
            else
                _wrappedEntity.Entity.CurrencyExchangeRate = 1;
        }

        private void comboCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            var oldCurrency = _wrappedEntity.Entity.CurrencyId;

            // update debt currency
            _wrappedEntity.Entity.Currency = _currencies.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.CurrencyId)?.ToReferenceView();
            
            if (_wrappedEntity.Entity.StorageCurrency != null && _wrappedEntity.Entity.StorageCurrency.Id != _wrappedEntity.Entity.CurrencyId)
            {
                // not update currency exchange rate if currencies was not changed
                if (_wrappedEntity.Entity.CurrencyId == oldCurrency)
                    return;

                // load _currencyExchangeRates and set correspond rate
                if (_currencyExchangeRates == null)
                {
                    ICurrencyExchangeRateService currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();

                    _currencyExchangeRates =
                        currencyExchangeRateService.GetList(
                            _storages.Select(_ => _.CurrencyId).Distinct().Concat(_currencies.Select(c => c.Id)).Distinct().ToList());
                }

                _wrappedEntity.Entity.CurrencyExchangeRate =
                    _currencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == _wrappedEntity.Entity.CurrencyId &&
                        _.CurrencyToId == _wrappedEntity.Entity.StorageCurrency.Id)?.Rate ?? 1;
            }
            else
                _wrappedEntity.Entity.CurrencyExchangeRate = 1;
        }

        #endregion

        #region Public

        public bool DialogResult { get; private set; } = false;

        public void SaveChanges()
        {
            if (_isNew)
                _service.Add(_wrappedEntity.Entity);
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

        #region Private methods

        private void FillPenalties()
        {
            _penaltyControl = new Dictionary<DebtPenaltyViewModel, ContentControl>();

            foreach (var penalty in _wrappedEntity.Entity.Penalties)
                AddPenaltyToView(penalty);
        }

        private void AddPenaltyToView(DebtPenaltyModel penalty)
        {
            // TODO: should be updated the same penalty object
            var penaltyViewModel = new DebtPenaltyViewModel(penalty)
            {
                DeleteCommand = DeletePenaltyCommand
            };

            var penaltyControl = new ContentControl()
            {
                Template = this.Resources["PenaltyItemControlTemplate"] as ControlTemplate,
                DataContext = penaltyViewModel
            };

            _penaltyControl.Add(penaltyViewModel, penaltyControl);
            PenaltiesPanel.Children.Add(penaltyControl);
        }

        #endregion
    }
}