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

        public MoneyTransferEventDetailsViewBase(IMoneyTransferEventService service, MoneyTransferEventModel entity, bool isNew)
            : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for MoneyTransferEventDetailsView.xaml
    /// </summary>
    public partial class MoneyTransferEventDetailsView : MoneyTransferEventDetailsViewBase
    {
        #region Private fields

        private IEnumerable<StorageModel> _storages;

        #endregion

        #region Initialization

        public MoneyTransferEventDetailsView(IMoneyTransferEventService service, MoneyTransferEventModel entity, bool isNew)
            : base(service, entity, isNew)
        {
            InitializeComponent();
            
            // load storages
            IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
            _storages = storageService.GetVisible(GlobalVariables.UserId, entity.StorageFromId, entity.StorageToId);

            comboFromStorage.ItemsSource = _storages;
            comboToStorage.ItemsSource = _storages;
            
            // set currencies list
            compCurrencyExchangeRate.CurrencyIds = _storages.Select(_ => _.CurrencyId).Distinct().ToList();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 600;
            window.Width = 770;
        }

        #endregion

        #region Event handlers

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!WrappedEntity.IsChanged) return;

            // update storages currency
            WrappedEntity.Entity.StorageFromCurrency = 
                _storages.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.StorageFromId)?.Currency;

            WrappedEntity.Entity.StorageToCurrency = _storages.FirstOrDefault(_ => _.Id == WrappedEntity.Entity.StorageToId)?.Currency;
        }

        #endregion
    }
}
