using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Enums;
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
    public abstract class MoneyTransferEventDetailsViewBase : BaseEntityDetailsView<MoneyTransferEventModel, MoneyTransferEventModel, IMoneyTransferEventService>
    {
        public MoneyTransferEventDetailsViewBase() : base()
        { }

        public MoneyTransferEventDetailsViewBase(IMoneyTransferEventService service, MoneyTransferEventModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for MoneyTransferEventDetailsView.xaml
    /// </summary>
    public partial class MoneyTransferEventDetailsView : MoneyTransferEventDetailsViewBase
    {
        #region Private fields

        private IEnumerable<StorageModel> _storages;
        private IEnumerable<CurrencyExchangeRateModel> _currencyExchangeRates;
        private ICurrencyExchangeRateService _currencyExchangeRateService;

        #endregion

        #region Initialization

        public MoneyTransferEventDetailsView(IMoneyTransferEventService service, MoneyTransferEventModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();

            // init
            _currencyExchangeRateService = ServiceManager.ConfigureService<CurrencyExchangeRateService>();
            
            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageFromId, entity.StorageToId);

            comboFromStorage.ItemsSource = _storages;
            comboToStorage.ItemsSource = _storages;

            // fill schedule selectors
            comboScheduleTypes.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(ScheduleType));
            comboDaysOfMonth.ItemsSource = ScheduleHelper.GetMonthes();
            DaysOfWeekControl.ItemsSource = MultiLangEnumHelper.ToSelectableCollection(entity.Schedule.DaysOfWeek);
            MonthesControl.ItemsSource = MultiLangEnumHelper.ToSelectableCollection(entity.Schedule.Months);

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers
        
        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            var oldCurrencyFrom = _wrappedEntity.Entity.StorageFromCurrency?.Id;
            var oldCurrencyTo = _wrappedEntity.Entity.StorageToCurrency?.Id;

            // update storages currency
            _wrappedEntity.Entity.StorageFromCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageFromId)?.Currency;

            _wrappedEntity.Entity.StorageToCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageToId)?.Currency;

            if (_wrappedEntity.Entity.StorageFromCurrency != null && _wrappedEntity.Entity.StorageToCurrency != null &&
                _wrappedEntity.Entity.StorageFromCurrency.Id != _wrappedEntity.Entity.StorageToCurrency.Id)
            {
                // not update currency exchange rate if currencies was not changed
                if (oldCurrencyFrom != null && oldCurrencyTo != null && oldCurrencyFrom.Value == _wrappedEntity.Entity.StorageFromCurrency.Id && oldCurrencyTo.Value == _wrappedEntity.Entity.StorageToCurrency.Id)
                    return;

                // load _currencyExchangeRates and set correspond rate
                if (_currencyExchangeRates == null)
                {
                    _currencyExchangeRates =
                        _currencyExchangeRateService.GetList(_storages.Select(_ => _.CurrencyId).Distinct().ToList());
                }

                _wrappedEntity.Entity.CurrencyExchangeRate =
                    _currencyExchangeRates.FirstOrDefault(_ => _.CurrencyFromId == _wrappedEntity.Entity.StorageFromCurrency.Id &&
                        _.CurrencyToId == _wrappedEntity.Entity.StorageToCurrency.Id)?.Rate ?? 1;
            }
            else
                _wrappedEntity.Entity.CurrencyExchangeRate = 1;
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

        #endregion
    }
}
