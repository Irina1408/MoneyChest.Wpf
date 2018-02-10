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
    public abstract class MoneyTransferDetailsViewBase : BaseEntityDetailsView<MoneyTransferModel, MoneyTransferModel, IMoneyTransferService>
    {
        public MoneyTransferDetailsViewBase() : base()
        { }

        public MoneyTransferDetailsViewBase(IMoneyTransferService service, MoneyTransferModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
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

        public MoneyTransferDetailsView(IMoneyTransferService service, MoneyTransferModel entity, bool isNew, Action closeAction,
            bool showHiddenStorages, IEnumerable<StorageModel> storages)
            : base(service, entity, isNew, closeAction)
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
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers

        private void comboStorage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_wrappedEntity.IsChanged) return;

            // update storages currency
            _wrappedEntity.Entity.StorageFromCurrency = 
                _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageFromId)?.Currency;

            _wrappedEntity.Entity.StorageToCurrency = _storages.FirstOrDefault(_ => _.Id == _wrappedEntity.Entity.StorageToId)?.Currency;
        }

        #endregion
    }
}
