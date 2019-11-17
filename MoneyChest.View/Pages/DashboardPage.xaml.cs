using MahApps.Metro.IconPacks;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Pages.DashboardItems;
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
    public partial class DashboardPage : PageBase
    {
        #region Private fields
        
        private List<IDashboardItem> dashboardItems;

        #endregion

        #region Initialization

        public DashboardPage() : base()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        protected override void InitializationComplete()
        {
            base.InitializationComplete();

            // get all dashboard items
            dashboardItems = new List<IDashboardItem>();
            System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(IDashboardItem)) && !mytype.IsAbstract)
                .ToList()
                .ForEach(t => dashboardItems.Add(Activator.CreateInstance(t) as IDashboardItem));

            // update items order
            dashboardItems = dashboardItems.OrderBy(x => x.Order).ToList();

            var iRow = 0;
            var iCol = 0;
            var maxCol = 3;

            // build view
            for (int i = 0; i < dashboardItems.Count; i++)
            {
                // attach actions
                dashboardItems[i].ReloadActual = () =>
                {
                    foreach(var item in dashboardItems.Where(x => x.ContainsActual))
                        item.Reload();
                };

                // configure item view
                Grid.SetRow(dashboardItems[i].View, iRow);
                Grid.SetColumn(dashboardItems[i].View, iCol);

                // add item into the view
                //DashboardItemsPanel.Children.Add(dashboardItem.View);
                DashboardItemsGrid.Children.Add(dashboardItems[i].View);

                // next row/column
                iCol = iCol == maxCol ? 0 : iCol + 1;
                iRow = iCol == 0 ? iRow + 1 : iRow;
            }
        }

        public override void Reload()
        {
            base.Reload();

            dashboardItems.ForEach(x => x.Reload());
        }

        #endregion
    }
}
