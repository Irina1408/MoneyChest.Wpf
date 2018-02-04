using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
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
    public abstract class StorageDetailsViewBase : BaseEntityDetailsView<StorageModel, StorageModel, IStorageService>
    {
        public StorageDetailsViewBase() : base()
        { }

        public StorageDetailsViewBase(IStorageService service, StorageModel entity, bool isNew, Action closeAction)
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for StorageDetailsView.xaml
    /// </summary>
    public partial class StorageDetailsView : StorageDetailsViewBase
    {
        #region Initialization

        public StorageDetailsView(IStorageService service, StorageModel entity, bool isNew, Action closeAction,
            IEnumerable<StorageGroupModel> storageGroups, IEnumerable<CurrencyModel> currencies)
            : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
            // initialize datacontexts
            comboStorageGroups.ItemsSource = storageGroups;
            comboCurrencies.ItemsSource = currencies;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion
    }
}
