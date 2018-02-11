using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using MoneyChest.ViewModel.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public abstract class CategoryDetailsViewBase : EntityDetailsViewBase<CategoryModel, CategoryModel, ICategoryService>
    {
        public CategoryDetailsViewBase() : base()
        { }

        public CategoryDetailsViewBase(ICategoryService service, CategoryModel entity, bool isNew) 
            : base(service, entity, isNew)
        { }
    }

    /// <summary>
    /// Interaction logic for CategoryDetailsView.xaml
    /// </summary>
    public partial class CategoryDetailsView : CategoryDetailsViewBase
    {        
        public CategoryDetailsView(ICategoryService service, CategoryModel entity, bool isNew, CategoryViewModelCollection categories) 
            : base(service, entity, isNew)
        {
            InitializeComponent();
            
            // init categories
            var cats = new CategoryViewModelCollection(categories);
            ParentCategorySelector.Categories = cats;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = Commands;
        }

        public override void PrepareParentWindow(Window window)
        {
            base.PrepareParentWindow(window);

            window.Height = 330;
            window.Width = 410;
        }
    }
}
