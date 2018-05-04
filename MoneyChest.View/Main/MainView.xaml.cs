using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;
using MoneyChest.View.Pages;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
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

namespace MoneyChest.View.Main
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        #region Private fields

        private MainViewModel _viewModel;

        #endregion

        #region Initialization

        public MainView()
        {
            InitializeComponent();

            // init
            _viewModel = new MainViewModel();

            // init context
            HamburgerMenuControl.DataContext = _viewModel;
        }

        #endregion

        #region Event handlers

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            // get all application pages
            var pages = new List<IPage>();
            System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(IPage)) && !mytype.IsAbstract)
                .ToList()
                .ForEach(t => pages.Add(Activator.CreateInstance(t) as IPage));

            // build hamburger menu
            foreach (var page in pages.OrderBy(_ => _.Order))
            {
                var menuItem = new CustomHamburgerMenuItem()
                {
                    Label = page.Label,
                    Tag = page.Icon,
                    BorderThickness = page.ShowTopBorder ? new Thickness(0, 1, 0, 0) : new Thickness(),
                    View = page.View
                };

                // add menu item into the menu
                if (page.IsOptionsPage) HamburgerMenuControl.OptionsItems.Add(menuItem);
                else HamburgerMenuControl.Items.Add(menuItem);

                // add events
                page.DataChanged += (s, arg) => pages.ForEach(p =>
                {
                    if (p != page)
                        p.RequiresReload = true;
                });
            }
        }

        private void HamburgerMenuControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            HamburgerMenuControl.Content = e.ClickedItem;
        }

        #endregion
    }
}
