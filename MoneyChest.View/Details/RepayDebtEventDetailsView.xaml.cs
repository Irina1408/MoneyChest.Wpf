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
    public abstract class RepayDebtEventDetailsViewBase : EntityDetailsViewBase<RepayDebtEventModel, RepayDebtEventViewModel, IRepayDebtEventService>
    {
        public RepayDebtEventDetailsViewBase() : base()
        { }

        public RepayDebtEventDetailsViewBase(IRepayDebtEventService service, RepayDebtEventViewModel entity, bool isNew)
            : base(service, entity, isNew)
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

        public RepayDebtEventDetailsView(IRepayDebtEventService service, RepayDebtEventViewModel entity, bool isNew)
            : base(service, entity, isNew)
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
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 600;
            window.Width = 780;
        }

        #endregion

        #region Event handlers

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;

            // update storage currency
            WrappedEntity.Entity.Storage = _storages.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.StorageId)?.ToReferenceView();
            WrappedEntity.Entity.StorageCurrency = _storages.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.StorageId)?.Currency;

            // update property IsValueInStorageCurrency
            UpdateIsValueInStorageCurrency();
            // update currencies combobox
            UpdateCurrenciesList();
        }

        private void comboDebt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;

            // update debt currency
            WrappedEntity.Entity.Debt = _debts.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.DebtId)?.ToReferenceView();
            WrappedEntity.Entity.DebtCurrency = _debts.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.DebtId)?.Currency;

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
            if (WrappedEntity.Entity.StorageCurrency?.Id == null || WrappedEntity.Entity.DebtCurrency?.Id == null) return;

            // get currencies of selected debt and storage
            comboCurrencies.ItemsSource = _currencies.Where(x => x.Id == WrappedEntity.Entity.StorageCurrency.Id
                || x.Id == WrappedEntity.Entity.DebtCurrency.Id).ToList();

            // update selected value
            comboCurrencies.SelectedValue = WrappedEntity.Entity.IsValueInStorageCurrency 
                ? WrappedEntity.Entity.StorageCurrency.Id
                : WrappedEntity.Entity.DebtCurrency.Id;
        }

        private void UpdateIsValueInStorageCurrency()
        {
            // get selected currency
            var currencyId = (int?)comboCurrencies.SelectedValue;
            // update property IsValueInStorageCurrency
            WrappedEntity.Entity.IsValueInStorageCurrency = WrappedEntity.Entity.StorageCurrency?.Id == currencyId;
        }

        #endregion
    }
}
