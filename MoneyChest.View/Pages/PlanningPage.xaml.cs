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
    /// Interaction logic for PlanningPage.xaml
    /// </summary>
    public partial class PlanningPage : UserControl, IPage
    {
        public PlanningPage()
        {
            InitializeComponent();
        }

        #region IPage implementation

        public string Label => "Planning";
        public FrameworkElement Icon { get; private set; } = new PackIconMaterial() { Kind = PackIconMaterialKind.CalendarClock };
        public int Order => 4;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion
    }
}
