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
    public abstract class CategoryDetailsViewBase : BaseEntityDetailsView<CategoryModel, CategoryModel, ICategoryService>
    {
        public CategoryDetailsViewBase() : base()
        { }

        public CategoryDetailsViewBase(ICategoryService service, CategoryModel entity, bool isNew, Action closeAction) 
            : base(service, entity, isNew, closeAction)
        { }
    }

    /// <summary>
    /// Interaction logic for CategoryDetailsView.xaml
    /// </summary>
    public partial class CategoryDetailsView : CategoryDetailsViewBase
    {
        #region Private fields
        
        private CategoryViewModelCollection _categories;

        #endregion

        #region Initialization
        
        public CategoryDetailsView(ICategoryService service, CategoryModel entity, bool isNew, Action closeAction, 
            CategoryViewModelCollection categories) : base(service, entity, isNew, closeAction)
        {
            InitializeComponent();
            
            // TODO: change category hierarchy
            // init categories
            _categories = new CategoryViewModelCollection(categories);

            // add empty category
            _categories.Insert(0, new CategoryViewModel()
            {
                Id = -1,
                Name = MultiLangResourceManager.Instance[MultiLangResourceName.None]
            });
            
            // set selected item in TreeView
            if(entity.ParentCategoryId.HasValue)
            {
                var parent = _categories.GetDescendants().FirstOrDefault(_ => _.Id == entity.ParentCategoryId.Value);
                parent.IsSelected = true;
                _categories.ExpandToDescendant(parent, true);
                txtParentCategory.Text = parent.Name;
            }
            else
            {
                var parent = _categories.GetDescendants().FirstOrDefault(_ => _.Id == -1);
                parent.IsSelected = true;
                txtParentCategory.Text = parent.Name;
            }
            
            TreeViewCategories.ItemsSource = _categories;

            // set header and commands panel context
            LabelHeader.Content = ViewHeader;
            CommandsPanel.DataContext = _commands;
        }

        #endregion

        #region Event handlers

        public void CategoryDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;

            var selectedParent = _categories.GetDescendants().FirstOrDefault(_ => _.IsSelected);
            if (selectedParent == null)
            {
                eventArgs.Cancel();
                return;
            }

            _wrappedEntity.Entity.ParentCategoryId = selectedParent.Id != -1 ? (int?)selectedParent.Id : null;
            txtParentCategory.Text = selectedParent.Name;
        }

        #endregion
    }
}
