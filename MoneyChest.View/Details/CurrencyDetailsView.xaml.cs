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
    public abstract class CurrencyDetailsViewBase : BaseEntityDetailsView<CurrencyModel, CurrencyModel, ICurrencyService>
    {
        public CurrencyDetailsViewBase() : base()
        { }

        public CurrencyDetailsViewBase(ICurrencyService service, CurrencyModel entity, bool isNew)
            : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for CurrencyDetailsView.xaml
    /// </summary>
    public partial class CurrencyDetailsView : CurrencyDetailsViewBase
    {
        public CurrencyDetailsView(ICurrencyService service, CurrencyModel entity, bool isNew)
            : base(service, entity, isNew)
        {
            InitializeComponent();
            
            // if currency is not new and main user cannot change it to not main
            btnIsMain.IsEnabled = isNew || !entity.IsMain;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 293;
            window.Width = 400;
        }
    }
}
