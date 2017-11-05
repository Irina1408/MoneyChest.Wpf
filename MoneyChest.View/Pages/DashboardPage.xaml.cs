using MahApps.Metro.IconPacks;
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

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    public partial class DashboardPage : UserControl, IPage
    {
        #region Initialization

        public DashboardPage()
        {
            InitializeComponent();
        }

        #endregion

        #region IPage implementation

        public string Label => "Dashboard";
        public FrameworkElement Icon { get; private set; } = new PackIconMaterial() { Kind = PackIconMaterialKind.ViewDashboard };
        public int Order => 1;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion
    }
}
