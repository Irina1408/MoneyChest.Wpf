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
        private IEnumerable<CurrencyExchangeRateModel> _currencyExchangeRates;

        #endregion

        #region Initialization

        public RepayDebtEventDetailsView(IRepayDebtEventService service, RepayDebtEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
            // load debts
            IDebtService debtService = ServiceManager.ConfigureService<DebtService>();
            _debts = debtService.GetActive(GlobalVariables.UserId);
            comboDebts.ItemsSource = _debts;

            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId);
            comboStorage.ItemsSource = _storages;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.Debt?.CurrencyId, entity.Storage?.CurrencyId);

            // fill schedule selectors
            comboScheduleTypes.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(ScheduleType));
            comboDaysOfMonth.ItemsSource = ScheduleHelper.GetMonthes();
            DaysOfWeekControl.ItemsSource = MultiLangEnumHelper.ToSelectableCollection(entity.Schedule.DaysOfWeek);
            MonthesControl.ItemsSource = MultiLangEnumHelper.ToSelectableCollection(entity.Schedule.Months);
            
            if (isNew)
                comboCurrencies.ItemsSource = _currencies;
            else
                UpdateCurrenciesList();
            
            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers
        
        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            var oldCurrencyId = _wrappedEntity.Entity.StorageCurrency?.Id;

            // update storage currency
            _wrappedEntity.Entity.StorageCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageId)?.Currency;

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();

            // if currency was changed
            if (_wrappedEntity.Entity.StorageCurrency != null && 
                (oldCurrencyId == null || _wrappedEntity.Entity.StorageCurrency.Id != oldCurrencyId.Value))
            {
                // update currencies combobox
                UpdateCurrenciesList();
                // update currency exchange rate 
                UpdateCurrencyExchangeRate();
            }
        }

        private void comboDebt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            var oldCurrencyId = _wrappedEntity.Entity.DebtCurrency?.Id;

            // update debt currency
            _wrappedEntity.Entity.DebtCurrency = _debts.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.DebtId)?.Currency;
            _wrappedEntity.Entity.Debt = _debts.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.DebtId)?.ToReferenceView();

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();

            // if currency was changed
            if (_wrappedEntity.Entity.DebtCurrency != null &&
                (oldCurrencyId == null || _wrappedEntity.Entity.DebtCurrency.Id != oldCurrencyId.Value))
            {
                // update currencies combobox
                UpdateCurrenciesList();
                // update currency exchange rate 
                UpdateCurrencyExchangeRate();
            }
        }

        private void comboCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboCurrencies.SelectedValue == null) return;

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();
            // update currency exchange rate 
            UpdateCurrencyExchangeRate();
        }

        private void DayOfWeek_CheckChanged(object sender, RoutedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            if (chkBox is null) return;

            var item = chkBox.DataContext as SelectableMultiLangEnumDescription;
            if (item is null) return;

            if (chkBox.IsChecked.HasValue && chkBox.IsChecked.Value)
                _wrappedEntity.Entity.Schedule.DaysOfWeek.Add((DayOfWeek)item.Value);
            else if (chkBox.IsChecked.HasValue && !chkBox.IsChecked.Value)
                _wrappedEntity.Entity.Schedule.DaysOfWeek.Remove((DayOfWeek)item.Value);

            _wrappedEntity.Entity.NotifyScheduleChanged();
        }

        private void Month_CheckChanged(object sender, RoutedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            if (chkBox is null) return;

            var item = chkBox.DataContext as SelectableMultiLangEnumDescription;
            if (item is null) return;

            if (chkBox.IsChecked.HasValue && chkBox.IsChecked.Value)
                _wrappedEntity.Entity.Schedule.Months.Add((Month)item.Value);
            else if (chkBox.IsChecked.HasValue && !chkBox.IsChecked.Value)
                _wrappedEntity.Entity.Schedule.Months.Remove((Month)item.Value);

            _wrappedEntity.Entity.NotifyScheduleChanged();
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

        private void UpdateCurrencyExchangeRate()
        {
            // update currency exchange rate
            if (_wrappedEntity.Entity.StorageCurrency != null && _wrappedEntity.Entity.DebtCurrency != null
                && _wrappedEntity.Entity.StorageCurrency.Id != _wrappedEntity.Entity.DebtCurrency.Id)
            {
                // load _currencyExchangeRates and set correspond rate
                if (_currencyExchangeRates == null)
                {
                    ICurrencyExchangeRateService currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();

                    _currencyExchangeRates =
                        currencyExchangeRateService.GetList(_debts.Select(_ => _.CurrencyId).Distinct().Concat(
                            _storages.Select(_ => _.CurrencyId).Distinct().Concat(_currencies.Select(c => c.Id))).Distinct().ToList());
                }

                _wrappedEntity.Entity.CurrencyExchangeRate =
                        _currencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == _wrappedEntity.Entity.Currency.Id &&
                            _.CurrencyToId == _wrappedEntity.Entity.CurrencyForRate.Id)?.Rate ?? 1;
            }
            else
                _wrappedEntity.Entity.CurrencyExchangeRate = 1;
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
