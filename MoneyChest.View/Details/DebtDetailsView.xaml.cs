using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Services.Services.Base;
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
    public abstract class DebtDetailsViewBase : BaseEntityDetailsView<DebtModel, DebtViewModel, IDebtService>
    {
        public DebtDetailsViewBase() : base()
        { }

        public DebtDetailsViewBase(IDebtService service, DebtViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for DebtDetailsView.xaml
    /// </summary>
    public partial class DebtDetailsView : DebtDetailsViewBase
    {
        #region Private fields
        
        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;
        private IEnumerable<CurrencyExchangeRateModel> _currencyExchangeRates;
        private ICommand DeletePenaltyCommand;

        private Dictionary<DebtPenaltyViewModel, ContentControl> _penaltyControl;

        #endregion

        #region Initialization

        public DebtDetailsView(IDebtService service, DebtViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
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
            
            FillPenalties();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        protected override void InitializeCommands()
        {
            base.InitializeCommands();

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
        }

        #endregion

        #region Event handlers

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
