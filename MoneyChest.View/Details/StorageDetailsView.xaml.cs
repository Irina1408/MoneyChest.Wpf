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
    public abstract class StorageDetailsViewBase : EntityDetailsViewBase<StorageModel, StorageModel, IStorageService>
    {
        public StorageDetailsViewBase() : base()
        { }

        public StorageDetailsViewBase(IStorageService service, StorageModel entity, bool isNew)
            : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for StorageDetailsView.xaml
    /// </summary>
    public partial class StorageDetailsView : StorageDetailsViewBase
    {
        public StorageDetailsView(IStorageService service, StorageModel entity, bool isNew,
            IEnumerable<StorageGroupModel> storageGroups, IEnumerable<CurrencyModel> currencies)
            : base(service, entity, isNew)
        {
            InitializeComponent();
            
            // initialize datacontexts
            comboStorageGroups.ItemsSource = storageGroups;
            comboCurrencies.ItemsSource = currencies;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 440;
            window.Width = 270;
        }
    }
}
