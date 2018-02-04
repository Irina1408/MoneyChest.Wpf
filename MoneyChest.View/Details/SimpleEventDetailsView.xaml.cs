using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Enums;
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
    public abstract class SimpleEventDetailsViewBase : BaseEntityDetailsView<SimpleEventModel, SimpleEventViewModel, ISimpleEventService>
    {
        public SimpleEventDetailsViewBase() : base()
        { }

        public SimpleEventDetailsViewBase(ISimpleEventService service, SimpleEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for SimpleEventDetailsView.xaml
    /// </summary>
    public partial class SimpleEventDetailsView : SimpleEventDetailsViewBase
    {
        #region Private fields

        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;
        private IEnumerable<CurrencyExchangeRateModel> _currencyExchangeRates;
        private CategoryViewModelCollection _categories;

        #endregion

        #region Initialization

        public SimpleEventDetailsView(ISimpleEventService service, SimpleEventViewModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId);
            comboStorage.ItemsSource = _storages;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.CurrencyId, entity.Storage?.CurrencyId);
            comboCurrencies.ItemsSource = _currencies;

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

            var oldCurrency = _wrappedEntity.Entity.StorageCurrency?.Id;

            // update storage currency
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
