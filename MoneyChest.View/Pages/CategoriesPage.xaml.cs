using MahApps.Metro.IconPacks;
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

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : UserControl, IPage
    {
        #region Private fields

        private ICategoryService _categoryService;
        private CategoriesPageViewModel _viewModel;

        #endregion

        #region Initialization

        public CategoriesPage()
        {
            InitializeComponent();

            // init
            _categoryService = ServiceManager.ConfigureService<CategoryService>();
            _viewModel = new CategoriesPageViewModel();
        }

        #endregion

        #region IPage implementation

        public string Label => "Categories";
        public FrameworkElement Icon { get; private set; } = new PackIconEntypo() { Kind = PackIconEntypoKind.FlowTree };
        public int Order => 9;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion

        #region Event handlers

        private void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Categories = _categoryService.GetListForUser(GlobalVariables.UserId);

            this.DataContext = _viewModel;
        }

        #endregion
    }

    public class CategoriesPageViewModel
    {
        public List<CategoryModel> Categories { get; set; }
    }
}
