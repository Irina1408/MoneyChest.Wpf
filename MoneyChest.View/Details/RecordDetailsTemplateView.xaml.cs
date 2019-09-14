using MoneyChest.Model.Enums;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
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
    public abstract class RecordTemplateDetailsViewBase : EntityDetailsViewBase<RecordTemplateModel, RecordTemplateModel, IRecordTemplateService>
    {
        public RecordTemplateDetailsViewBase() : base()
        { }

        public RecordTemplateDetailsViewBase(IRecordTemplateService service, RecordTemplateModel entity, bool isNew, bool? allowSaveIfNoChanges = null)
            : base(service, entity, isNew, allowSaveIfNoChanges)
        { }
    }

    /// <summary>
    /// Interaction logic for RecordTemplateDetailsView.xaml
    /// </summary>
    public partial class RecordTemplateDetailsView : RecordTemplateDetailsViewBase
    {
        #region Private fields

        private IEnumerable<CurrencyModel> _currencies;
        private IEnumerable<StorageModel> _storages;
        private IEnumerable<DebtModel> _debts;

        #endregion

        #region Initialization

        public RecordTemplateDetailsView(RecordTemplateModel entity, bool isNew)
            : this(ServiceManager.ConfigureService<RecordTemplateService>(), entity, isNew)
        { }

        public RecordTemplateDetailsView(IRecordTemplateService service, RecordTemplateModel entity, bool isNew, bool? allowSaveIfNoChanges = null)
            : base(service, entity, isNew, allowSaveIfNoChanges)
        {
            InitializeComponent();

            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageId);
            comboStorage.ItemsSource = _storages;

            // load currencies
            ICurrencyService currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _currencies = currencyService.GetActive(GlobalVariables.UserId, entity.CurrencyId, entity.Storage?.CurrencyId, entity.Debt?.CurrencyId);
            CurrencySelector.Currencies = _currencies;

            // load debts
            IDebtService debtService = ServiceManager.ConfigureService<DebtService>();
            _debts = debtService.GetActive(GlobalVariables.UserId, entity.DebtId);
            comboDebts.ItemsSource = _debts;

            // set currencies list
            compCurrencyExchangeRate.CurrencyIds =
                _storages.Select(_ => _.CurrencyId).Distinct().Concat(_currencies.Select(c => c.Id)).Distinct().ToList();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 575;
            window.Width = 580;
        }

        #endregion

        #region Event handlers

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;

            // update storage reference
            WrappedEntity.Entity.Storage = _storages.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.StorageId)?.ToReferenceView();
            // update currencies combobox
            UpdateCurrenciesList();
        }

        //private void comboCurrencies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (!WrappedEntity.IsChanged) return;
        //    WrappedEntity.Entity.Currency = _currencies.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.CurrencyId)?.ToReferenceView();
        //}

        private void comboDebt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;

            if(WrappedEntity.Entity.DebtId.HasValue)
            {
                // set selected debt and update related fields
                var selectedDebt = _debts.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.DebtId);
                // fill record type
                WrappedEntity.Entity.RecordType = selectedDebt.DebtType == DebtType.TakeBorrow ? RecordType.Expense : RecordType.Income;
                // fill description if required
                if (!string.IsNullOrEmpty(selectedDebt.Description))
                    WrappedEntity.Entity.Description = selectedDebt.Description;
                // TODO: fill value to be paid in the next iteration
                WrappedEntity.Entity.Value = selectedDebt.ValueToBePaid;
                // fill currency
                WrappedEntity.Entity.CurrencyId = selectedDebt.CurrencyId;
                // fill category
                // TODO: make sure category is changed in view
                if (selectedDebt.CategoryId.HasValue)
                    WrappedEntity.Entity.CategoryId = selectedDebt.CategoryId;
            }

            // update debt reference
            WrappedEntity.Entity.Debt = _debts.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.DebtId)?.ToReferenceView();
            // update currencies combobox
            UpdateCurrenciesList();
        }

        private void UpdateCurrenciesList()
        {
            if (WrappedEntity.Entity.DebtId.HasValue)
            {
                // select correspond currency
                if (WrappedEntity.Entity.CurrencyId > 0 && WrappedEntity.Entity.CurrencyId != WrappedEntity.Entity.Debt.CurrencyId
                    && WrappedEntity.Entity.CurrencyId != WrappedEntity.Entity.Storage.CurrencyId)
                    WrappedEntity.Entity.CurrencyId = WrappedEntity.Entity.Debt.CurrencyId;

                // limit currencies
                CurrencySelector.Currencies = _currencies.Where(x => x.Id == WrappedEntity.Entity.Debt.CurrencyId
                    || x.Id == WrappedEntity.Entity.Storage.CurrencyId).ToList();
            }
            else
                CurrencySelector.Currencies = _currencies;
        }

        #endregion
    }
}
