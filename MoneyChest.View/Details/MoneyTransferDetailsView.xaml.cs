using MaterialDesignThemes.Wpf;
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
    public abstract class MoneyTransferDetailsViewBase : EntityDetailsViewBase<MoneyTransferModel, MoneyTransferModel, IMoneyTransferService>
    {
        public MoneyTransferDetailsViewBase() : base()
        { }

        public MoneyTransferDetailsViewBase(IMoneyTransferService service, MoneyTransferModel entity, bool isNew)
            : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for MoneyTransferDetailsView.xaml
    /// </summary>
    public partial class MoneyTransferDetailsView : MoneyTransferDetailsViewBase
    {
        #region Private fields
        
        private IEnumerable<StorageModel> _storages;
        private bool _showHiddenStorages;

        #endregion

        #region Initialization

        public MoneyTransferDetailsView(MoneyTransferModel entity, bool isNew, bool showHiddenStorages)
            : this(ServiceManager.ConfigureService<MoneyTransferService>(), entity, isNew, showHiddenStorages,
                  (ServiceManager.ConfigureService<StorageService>() as IStorageService).GetListForUser(GlobalVariables.UserId))
        { }

        public MoneyTransferDetailsView(MoneyTransferModel entity, bool isNew,
            bool showHiddenStorages, IEnumerable<StorageModel> storages)
            : this(ServiceManager.ConfigureService<MoneyTransferService>(), entity, isNew, showHiddenStorages, storages)
        { }

        public MoneyTransferDetailsView(IMoneyTransferService service, MoneyTransferModel entity, bool isNew,
            bool showHiddenStorages, IEnumerable<StorageModel> storages)
            : base(service, entity, isNew)
        {
            InitializeComponent();

            // init
            _showHiddenStorages = showHiddenStorages;
            _storages = storages;

            // initialize datacontexts
            IEnumerable<StorageModel> showStorages;
            if (_showHiddenStorages)
                showStorages = storages;
            else
                showStorages = storages.Where(_ => _.IsVisible || entity.StorageFromId == _.Id || entity.StorageToId == _.Id);

            comboFromStorage.ItemsSource = showStorages;
            comboToStorage.ItemsSource = showStorages;

            // set currencies list
            compCurrencyExchangeRate.CurrencyIds = _storages.Select(_ => _.CurrencyId).Distinct().ToList();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 450;
            window.Width = 800;
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
