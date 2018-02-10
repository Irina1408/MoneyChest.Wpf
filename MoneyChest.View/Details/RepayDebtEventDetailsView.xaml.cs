using MoneyChest.Model.Model;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
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
    public abstract class RepayDebtEventDetailsViewBase : BaseEntityDetailsView<RepayDebtEventModel, RepayDebtEventViewModel, IRepayDebtEventService>
    {
        public RepayDebtEventDetailsViewBase() : base()
        { }

        public RepayDebtEventDetailsViewBase(IRepayDebtEventService service, RepayDebtEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for RepayDebtEventDetailsView.xaml
    /// </summary>
    public partial class RepayDebtEventDetailsView : RepayDebtEventDetailsViewBase
    {
        #region Private fields

        private IEnumerable<DebtModel> _debts;
        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;

        #endregion

        #region Initialization

        public RepayDebtEventDetailsView(IRepayDebtEventService service, RepayDebtEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
            // load debts
            IDebtService debtService = ServiceManager.ConfigureService<DebtService>();
            // TODO: add required debts
            _debts = debtService.GetActive(GlobalVariables.UserId);
            comboDebts.ItemsSource = _debts;

            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId);
            comboStorage.ItemsSource = _storages;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.Debt?.CurrencyId, entity.Storage?.CurrencyId);
                        
            if (isNew)
                comboCurrencies.ItemsSource = _currencies;
            else
                UpdateCurrenciesList();

            // set currencies list
            compCurrencyExchangeRate.CurrencyIds = _storages.Select(_ => _.CurrencyId)
                .Concat(_currencies.Select(c => c.Id)).Concat(_debts.Select(c => c.CurrencyId)).Distinct().ToList();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers
        
        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            // update storage currency
            _wrappedEntity.Entity.Storage = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageId)?.ToReferenceView();
            _wrappedEntity.Entity.StorageCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageId)?.Currency;

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();
            // update currencies combobox
            UpdateCurrenciesList();
        }

        private void comboDebt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            // update debt currency
            _wrappedEntity.Entity.Debt = _debts.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.DebtId)?.ToReferenceView();
            _wrappedEntity.Entity.DebtCurrency = _debts.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.DebtId)?.Currency;

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();
            // update currencies combobox
            UpdateCurrenciesList();
        }

        private void comboCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCurrencies.SelectedValue == null) return;

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();
        }
        
        private void UpdateCurrenciesList()
        {
            if (_wrappedEntity.Entity.StorageCurrency?.Id == null || _wrappedEntity.Entity.DebtCurrency?.Id == null) return;

            // get currencies of selected debt and storage
            comboCurrencies.ItemsSource = _currencies.Where(x => x.Id == _wrappedEntity.Entity.StorageCurrency.Id
                || x.Id == _wrappedEntity.Entity.DebtCurrency.Id).ToList();

            // update selected value
            comboCurrencies.SelectedValue = _wrappedEntity.Entity.IsValueInStorageCurrency 
                ? _wrappedEntity.Entity.StorageCurrency.Id
                : _wrappedEntity.Entity.DebtCurrency.Id;
        }

        private void UpdateIsValueInStorageCurrency()
        {
            // get selected currency
            var currencyId = (int?)comboCurrencies.SelectedValue;
            // update property IsValueInStorageCurrency
            _wrappedEntity.Entity.IsValueInStorageCurrency = _wrappedEntity.Entity.StorageCurrency?.Id == currencyId;
        }

        #endregion
    }
}
