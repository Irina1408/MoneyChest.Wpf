using MaterialDesignThemes.Wpf;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.Extensions;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MoneyChest.View.Components
{
    /// <summary>
    /// Interaction logic for CategorySelector.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class CategorySelector : UserControl
    {
        private const int EmptyCategoryId = -1;
        private ICategoryService categoryService;

        public CategorySelector()
        {
            InitializeComponent();

            // init
            categoryService = ServiceManager.ConfigureService<CategoryService>();
        }

        #region Event handlers

        private void CategorySelector_Loaded(object sender, RoutedEventArgs e)
        {
            // init commands
            ExpandAllCommand = new Command(() => Categories.ExpandAll(true));
            CollapseAllCommand = new Command(() => Categories.ExpandAll(false));
            AddCommand = new Command(() => OpenDetails(new CategoryViewModel() { UserId = GlobalVariables.UserId }, true));

            AddChildCommand = new TreeViewSelectedItemCommand<CategoryViewModel>(TreeViewCategories,
            (item) => OpenDetails(new CategoryViewModel()
            {
                UserId = GlobalVariables.UserId,
                ParentCategoryId = item.Id,
                RecordType = item.RecordType,
                IsActive = item.IsActive
            }, true));

            // init datacontext
            TreeViewCategories.DataContext = this;
            MainToolBarTray.DataContext = this;
        }

        public void CategoryDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (!Equals(eventArgs.Parameter, "1")) return;

            var selectedCategory = Categories.GetDescendants().FirstOrDefault(_ => _.IsSelected);
            if (selectedCategory == null)
            {
                eventArgs.Cancel();
                return;
            }

            SelectedCategoryId = selectedCategory.Id != EmptyCategoryId ? (int?)selectedCategory.Id : null;
        }

        #endregion

        #region Hint Property

        public string Hint
        {
            get => (string)this.GetValue(HintProperty);
            set => this.SetValue(HintProperty, value);
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
            nameof(Hint), typeof(string), typeof(CategorySelector),
            new FrameworkPropertyMetadata(MultiLangResourceManager.Instance[MultiLangResourceName.Singular("Category")], HintChangedCallback));

        private static void HintChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint((d as CategorySelector).txtCategory, e.NewValue);
        }

        #endregion

        #region ShowEmptyCategory Property

        public bool ShowEmptyCategory
        {
            get => (bool)this.GetValue(ShowEmptyCategoryProperty);
            set => this.SetValue(ShowEmptyCategoryProperty, value);
        }

        public static readonly DependencyProperty ShowEmptyCategoryProperty = DependencyProperty.Register(
            nameof(ShowEmptyCategory), typeof(bool), typeof(CategorySelector),
            new FrameworkPropertyMetadata(false, ShowEmptyCategoryChangedCallback));

        private static void ShowEmptyCategoryChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var categorySelector = (d as CategorySelector);
            // get empty category
            var emptyCategory = categorySelector.Categories.GetDescendants().FirstOrDefault(_ => _.Id == EmptyCategoryId);
            if ((bool)e.NewValue && emptyCategory == null)
            {
                // insert new empty category
                categorySelector.Categories.Insert(0, new CategoryViewModel()
                {
                    Id = -1,
                    Name = MultiLangResourceManager.Instance[MultiLangResourceName.None]
                });
            }
            else if (!(bool)e.NewValue && emptyCategory != null)
                // remove empty category
                categorySelector.Categories.RemoveDescendant(emptyCategory);
        }

        #endregion

        #region SelectedCategoryId Property

        public int? SelectedCategoryId
        {
            get => (int?) this.GetValue(SelectedCategoryIdProperty);
            set => this.SetValue(SelectedCategoryIdProperty, value);
        }

        public static readonly DependencyProperty SelectedCategoryIdProperty = DependencyProperty.Register(
            nameof(SelectedCategoryId), typeof(int?), typeof(CategorySelector),
            new FrameworkPropertyMetadata(null, SelectedCategoryIdChangedCallback));

        private static void SelectedCategoryIdChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var categorySelector = (d as CategorySelector);
            // prepare selected category id
            var selectedCategoryId = (int?)e.NewValue ?? EmptyCategoryId;
            // get categories
            var categories = categorySelector.Categories.GetDescendants();
            // find selected category
            var selectedCategory = categories.FirstOrDefault(_ => _.Id == selectedCategoryId);
            if (selectedCategory == null) return;
            // unselect all categories except new selected category
            foreach (var cat in categories)
                cat.IsSelected = cat.Id == selectedCategoryId;
            // show category in hierarchy
            categorySelector.Categories.ExpandToDescendant(selectedCategory, true);
            // update shown category name
            (d as CategorySelector).txtCategory.Text = selectedCategory?.Id != -1 ? selectedCategory?.Name : null;
            (d as CategorySelector).txtCategoryBlock.Text = selectedCategory?.Id != -1 ? selectedCategory?.Name : null;
        }

        #endregion

        #region Categories Property

        public CategoryViewModelCollection Categories
        {
            get
            {
                if((CategoryViewModelCollection)this.GetValue(CategoriesProperty) == null)
                {
                    // load categories
                    //var categories = TreeHelper.BuildTree(categoryService.GetActive(GlobalVariables.UserId, SelectedCategoryId)
                    //    .OrderByDescending(_ => _.RecordType)
                    //    .ThenBy(_ => _.Name)
                    //    .ToList(), SelectedCategoryId, ShowEmptyCategory);

                    this.SetValue(CategoriesProperty, LoadCategories());
                }

                return (CategoryViewModelCollection)this.GetValue(CategoriesProperty);
            }
            set => this.SetValue(CategoriesProperty, value);
        }

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register(
            nameof(Categories), typeof(CategoryViewModelCollection), typeof(CategorySelector));

        #endregion

        #region SmallMode Property

        public bool SmallMode
        {
            get => (bool)this.GetValue(SmallModeProperty);
            set => this.SetValue(SmallModeProperty, value);
        }

        public static readonly DependencyProperty SmallModeProperty = DependencyProperty.Register(
            nameof(SmallMode), typeof(bool), typeof(CategorySelector), 
            new FrameworkPropertyMetadata(false, SmallModeBlockChangedCallback));

        private static void SmallModeBlockChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var categorySelector = (d as CategorySelector);
            // update visibility
            (d as CategorySelector).txtCategory.Visibility = categorySelector.SmallMode ? Visibility.Collapsed : Visibility.Visible;
            (d as CategorySelector).txtCategoryBlock.Visibility = categorySelector.SmallMode ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region ShowIcon Property

        public bool ShowIcon
        {
            get => (bool)this.GetValue(ShowIconProperty);
            set => this.SetValue(ShowIconProperty, value);
        }

        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(
            nameof(ShowIcon), typeof(bool), typeof(CategorySelector),
            new FrameworkPropertyMetadata(true));

        #endregion

        #region ShowInactive Property

        public bool ShowInactive
        {
            get => (bool)this.GetValue(ShowInactiveProperty);
            set => this.SetValue(ShowInactiveProperty, value);
        }

        public static readonly DependencyProperty ShowInactiveProperty = DependencyProperty.Register(
            nameof(ShowInactive), typeof(bool), typeof(CategorySelector),
            new FrameworkPropertyMetadata(false, ShowInactiveBlockChangedCallback));

        private static void ShowInactiveBlockChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var categorySelector = (d as CategorySelector);
            // reload categories list
            categorySelector.Categories = categorySelector.LoadCategories();
        }

        #endregion

        #region Commands
        
        public IMCCommand AddCommand { get; set; }
        public IMCCommand AddChildCommand { get; set; }
        public ICommand ExpandAllCommand { get; set; }
        public ICommand CollapseAllCommand { get; set; }

        #endregion

        #region Private methods

        private void OpenDetails(CategoryViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new CategoryDetailsView(categoryService, model, isNew, Categories), () =>
            {
                // set expanded branch where category was changed
                Categories.ExpandToDescendant(model, true);
                model.IsSelected = true;

                // reload data
                Categories = TreeHelper.BuildTree(categoryService.GetActive(GlobalVariables.UserId, SelectedCategoryId)
                        .OrderByDescending(_ => _.RecordType)
                        .ThenBy(_ => _.Name)
                        .ToList(), SelectedCategoryId, ShowEmptyCategory);
            });
        }

        private CategoryViewModelCollection LoadCategories()
        {
            var categories = ShowInactive 
                ? categoryService.GetListForUser(GlobalVariables.UserId)
                : categoryService.GetActive(GlobalVariables.UserId, SelectedCategoryId);

            return TreeHelper.BuildTree(categories
                .OrderByDescending(_ => _.RecordType)
                .ThenBy(_ => _.Name)
                .ToList(), SelectedCategoryId, ShowEmptyCategory);
        }

        #endregion
    }
}
