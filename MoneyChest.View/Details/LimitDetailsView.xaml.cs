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
    public abstract class LimitDetailsViewBase : EntityDetailsViewBase<LimitModel, LimitModel, ILimitService>
    {
        public LimitDetailsViewBase() : base()
        { }

        public LimitDetailsViewBase(ILimitService service, LimitModel entity, bool isNew) : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for LimitDetailsView.xaml
    /// </summary>
    public partial class LimitDetailsView : LimitDetailsViewBase
    {
        public LimitDetailsView(ILimitService service, LimitModel entity, bool isNew) : base(service, entity, isNew)
        {
            InitializeComponent();

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 355;
            window.Width = 510;
        }
    }
}
