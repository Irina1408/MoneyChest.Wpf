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
    /// Interaction logic for CategoryListSelector.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class CategoryListSelector : UserControl
    {
        private const int EmptyCategoryId = -1;
        private ICategoryService categoryService;
        private List<int> _selectedCategoryIds = new List<int>();
        private string label;

        public CategoryListSelector()
        {
            InitializeComponent();

            // init
            categoryService = ServiceManager.ConfigureService<CategoryService>();
        }

        #region Event handlers

        private void CategoryListSelector_Loaded(object sender, RoutedEventArgs e)
        {
            // init commands
            ExpandAllCommand = new Command(() => Categories.ExpandAll(true));
            CollapseAllCommand = new Command(() => Categories.ExpandAll(false));

            SelectAllCommand = new Command(() => Categories.SelectAll(true));
            UnselectAllCommand = new Command(() => Categories.SelectAll(false));

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

            SelectedCategoryIds = _selectedCategoryIds.ToList();
        }

        #endregion

        #region Hint Property

        public string Hint
        {
            get => (string)this.GetValue(HintProperty);
            set => this.SetValue(HintProperty, value);
        }

        public static readonly DependencyProperty HintProperty = DependencyProperty.Register(
            nameof(Hint), typeof(string), typeof(CategoryListSelector),
            new FrameworkPropertyMetadata(MultiLangResourceManager.Instance[MultiLangResourceName.Plural("Category")], HintChangedCallback));

        private static void HintChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MaterialDesignThemes.Wpf.HintAssist.SetHint((d as CategoryListSelector).txtCategory, e.NewValue);
        }

        #endregion

        #region SelectedCategoryIds Property

        public List<int> SelectedCategoryIds
        {
            get => (List<int>)this.GetValue(SelectedCategoryIdsProperty);
            set => this.SetValue(SelectedCategoryIdsProperty, value);
        }

        public static readonly DependencyProperty SelectedCategoryIdsProperty = DependencyProperty.Register(
            nameof(SelectedCategoryIds), typeof(List<int>), typeof(CategoryListSelector),
            new FrameworkPropertyMetadata(null, SelectedCategoryIdsChangedCallback));

        private static void SelectedCategoryIdsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // set local selected category ids
            (d as CategoryListSelector)._selectedCategoryIds = (e.NewValue as List<int>)?.ToList() ?? new List<int>();
            // refresh selected categories
            (d as CategoryListSelector).ApplySelectedCategories();

            //// get selector
            //var categoryListSelector = (d as CategoryListSelector);
            //// prepare selected category id
            //var selectedCategoryIds = (List<int>)e.NewValue ?? new List<int>();
            //// get categories
            //var categories = categoryListSelector.Categories.GetDescendants();
            //// unselect all categories except new selected category
            //foreach (var cat in categories)
            //    cat.IsSelected = selectedCategoryIds.Contains(cat.Id);
            //// update shown category name or categories count
            //// TODO: language
            //var categoriesLabel = selectedCategoryIds.Count == 0 || selectedCategoryIds.Count == categories.Count() ? "All"
            //    : selectedCategoryIds.Count == 1 ? categories.FirstOrDefault(x => x.Id == selectedCategoryIds[0])?.Name
            //    : $"{selectedCategoryIds.Count} categories";
            //(d as CategoryListSelector).txtCategory.Text = categoriesLabel;
            //(d as CategoryListSelector).txtCategoryBlock.Text = categoriesLabel;
        }

        #endregion

        #region Categories Property

        public CategoryViewModelCollection Categories
        {
            get
            {
                if ((CategoryViewModelCollection)this.GetValue(CategoriesProperty) == null)
                    this.SetValue(CategoriesProperty, LoadCategories());

                return (CategoryViewModelCollection)this.GetValue(CategoriesProperty);
            }
            set => this.SetValue(CategoriesProperty, value);
        }

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register(
            nameof(Categories), typeof(CategoryViewModelCollection), typeof(CategoryListSelector),
            new FrameworkPropertyMetadata(null, CategoriesChangedCallback));

        private static void CategoriesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // refresh selected categories
            (d as CategoryListSelector).ApplySelectedCategories();
        }

        #endregion

        #region SmallMode Property

        public bool SmallMode
        {
            get => (bool)this.GetValue(SmallModeProperty);
            set => this.SetValue(SmallModeProperty, value);
        }

        public static readonly DependencyProperty SmallModeProperty = DependencyProperty.Register(
            nameof(SmallMode), typeof(bool), typeof(CategoryListSelector),
            new FrameworkPropertyMetadata(false, SmallModeBlockChangedCallback));

        private static void SmallModeBlockChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var categoryListSelector = (d as CategoryListSelector);
            // update visibility
            (d as CategoryListSelector).txtCategory.Visibility = categoryListSelector.SmallMode ? Visibility.Collapsed : Visibility.Visible;
            (d as CategoryListSelector).txtCategoryBlock.Visibility = categoryListSelector.SmallMode ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region ShowIcon Property

        public bool ShowIcon
        {
            get => (bool)this.GetValue(ShowIconProperty);
            set => this.SetValue(ShowIconProperty, value);
        }

        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register(
            nameof(ShowIcon), typeof(bool), typeof(CategoryListSelector),
            new FrameworkPropertyMetadata(true));

        #endregion

        #region ShowInactive Property

        public bool ShowInactive
        {
            get => (bool)this.GetValue(ShowInactiveProperty);
            set => this.SetValue(ShowInactiveProperty, value);
        }

        public static readonly DependencyProperty ShowInactiveProperty = DependencyProperty.Register(
            nameof(ShowInactive), typeof(bool), typeof(CategoryListSelector),
            new FrameworkPropertyMetadata(false, ShowInactiveBlockChangedCallback));

        private static void ShowInactiveBlockChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get selector
            var categoryListSelector = (d as CategoryListSelector);
            // reload categories list
            categoryListSelector.Categories = categoryListSelector.LoadCategories();
        }

        #endregion

        #region Commands

        public IMCCommand AddCommand { get; set; }
        public IMCCommand AddChildCommand { get; set; }

        public ICommand SelectAllCommand { get; set; }
        public ICommand UnselectAllCommand { get; set; }

        public ICommand ExpandAllCommand { get; set; }
        public ICommand CollapseAllCommand { get; set; }

        #endregion

        #region Additional properties

        public bool IsCategoryBranchSelection { get; set; }

        #endregion

        #region Private methods

        private void OpenDetails(CategoryViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new CategoryDetailsView(categoryService, model, isNew, Categories), () =>
            {
                // reload data
                Categories = TreeHelper.BuildTree(categoryService.GetActive(GlobalVariables.UserId,
                    _selectedCategoryIds.Select(x => (int?)x).ToArray())
                        .OrderByDescending(_ => _.RecordType)
                        .ThenBy(_ => _.Name)
                        .ToList(), _selectedCategoryIds, true);

                // set expanded branch where category was changed
                Categories.ExpandToDescendant(model, true);
            });
        }

        private CategoryViewModelCollection LoadCategories()
        {
            var categories = ShowInactive
                ? categoryService.GetListForUser(GlobalVariables.UserId)
                : categoryService.GetActive(GlobalVariables.UserId, _selectedCategoryIds.Select(x => (int?)x).ToArray());

            return TreeHelper.BuildTree(categories
                .OrderByDescending(_ => _.RecordType)
                .ThenBy(_ => _.Name)
                .ToList(), _selectedCategoryIds, true);
        }

        #endregion

        #region Private methods (Categories)

        private bool isCatsApplying = false;
        private bool isSelectionChanged = false;

        private void ApplySelectedCategories()
        {
            if (!isCatsApplying && !isSelectionChanged)
            {
                isCatsApplying = true;
                var allSelected = false;

                foreach (var cat in Categories.GetDescendants())
                {
                    // set selection
                    cat.IsSelected = _selectedCategoryIds.Count == 0 || _selectedCategoryIds.Contains(cat.Id);
                    // expand category in the tree if it's selected
                    if (cat.IsSelected) Categories.ExpandToDescendant(cat, true);
                    // add event on category selection changed
                    cat.PropertyChanged += (sender, e) =>
                    {
                        if (e.PropertyName == nameof(CategoryViewModel.IsSelected) && !isSelectionChanged && !isCatsApplying)
                        {
                            isSelectionChanged = true;
                            // select full branch when IsCategoryBranchSelection
                            if (IsCategoryBranchSelection)
                                cat.Children.GetDescendants().ToList().ForEach(x => x.IsSelected = cat.IsSelected);
                            // get all categories
                            var allCats = Categories.GetDescendants().ToList();
                            // check all categories selected
                            allSelected = allCats.Count == allCats.Where(x => x.IsSelected).Count();

                            // update selected categories list
                            _selectedCategoryIds = allSelected ? new List<int>() : allCats.Where(x => x.IsSelected).Select(x => x.Id).ToList();

                            // update shown category name or categories count
                            var categoriesLabel = GetCategoriesLabel(allSelected, allCats);
                            txtCategory.Text = categoriesLabel;
                            txtCategoryBlock.Text = categoriesLabel;

                            isSelectionChanged = false;
                        }
                    };
                }

                // get all categories
                var allcats = Categories.GetDescendants().ToList();
                // check all categories selected
                allSelected = allcats.Count == allcats.Where(x => x.IsSelected).Count();
                // collapse all if all are selected 
                if (allSelected)
                    allcats.ForEach(x => x.IsExpanded = false);
                else
                    allcats.ForEach(x =>
                    {
                        // expand only selected
                        if (x.IsSelected)
                            Categories.ExpandToDescendant(x, true);
                    });

                // update shown category name or categories count
                var catsLabel = GetCategoriesLabel(allSelected, allcats);
                txtCategory.Text = catsLabel;
                txtCategoryBlock.Text = catsLabel;

                isCatsApplying = false;
            }
        }

        private string GetCategoriesLabel(bool allSelected, IEnumerable<CategoryViewModel> categories)
        {
            // if all categories are selected show 'All'
            if (allSelected) return MultiLangResourceManager.Instance[MultiLangResourceName.All];
            // if only one category is selected show category name
            if (_selectedCategoryIds.Count == 1)
                return categories.FirstOrDefault(x => x.Id == _selectedCategoryIds[0])?.Name;
            // in all other cases show categories count
            return $"{_selectedCategoryIds.Count} {MultiLangResourceManager.Instance[MultiLangResourceName.Plural(typeof(CategoryViewModel))].ToLower()}";
        }

        #endregion
    }
}
