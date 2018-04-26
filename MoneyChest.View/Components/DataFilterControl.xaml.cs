using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
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
    /// Interaction logic for DataFilterControl.xaml
    /// </summary>
    public partial class DataFilterControl : UserControl
    {
        public DataFilterControl()
        {
            InitializeComponent();
        }

        #region Event handlers

        private void DataFilter_Loaded(object sender, RoutedEventArgs e)
        {
            // init commands
            ExpandAllCommand = new Command(() => Categories.ExpandAll());
            CollapseAllCommand = new Command(() => Categories.CollapseAll());

            // init datacontext
            TreeViewCategories.DataContext = this;
        }

        #endregion

        #region DataFilter Property

        public DataFilterModel DataFilter
        {
            get => (DataFilterModel)this.GetValue(DataFilterProperty);
            set => this.SetValue(DataFilterProperty, value);
        }

        public static readonly DependencyProperty DataFilterProperty = DependencyProperty.Register(
            nameof(DataFilter), typeof(DataFilterModel), typeof(DataFilterControl),
            new FrameworkPropertyMetadata(null, DataFilterChangedCallback));

        private static void DataFilterChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get filter
            var filter = (d as DataFilterControl);
            filter.ApplySelectedCategories();
            filter.ApplySelectedStorages();
        }

        #endregion
        
        #region Categories Property

        public CategoryViewModelCollection Categories
        {
            get
            {
                if ((CategoryViewModelCollection)this.GetValue(CategoriesProperty) == null)
                {
                    // load categories
                    ICategoryService categoryService = ServiceManager.ConfigureService<CategoryService>();
                    var categories = TreeHelper.BuildTree(categoryService.GetListForUser(GlobalVariables.UserId)
                        .OrderByDescending(_ => _.RecordType)
                        .ThenBy(_ => _.Name)
                        .ToList(), true);

                    this.SetValue(CategoriesProperty, categories);
                }

                return (CategoryViewModelCollection)this.GetValue(CategoriesProperty);
            }
            set => this.SetValue(CategoriesProperty, value);
        }

        public static readonly DependencyProperty CategoriesProperty = DependencyProperty.Register(
            nameof(Categories), typeof(CategoryViewModelCollection), typeof(DataFilterControl),
            new FrameworkPropertyMetadata(null, CategoriesChangedCallback));

        private static void CategoriesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get filter
            var filter = (d as DataFilterControl);
            filter.ApplySelectedCategories();
        }

        #endregion

        #region Storages Property

        public List<StorageModel> Storages
        {
            get
            {
                if ((List<StorageModel>)this.GetValue(StoragesProperty) == null)
                {
                    // load storages
                    IStorageService storageService = ServiceManager.ConfigureService<StorageService>();
                    var storages = storageService.GetListForUser(GlobalVariables.UserId)
                        .OrderBy(_ => _.StorageGroup.Name)
                        .ThenBy(_ => _.Name)
                        .ToList();

                    this.SetValue(StoragesProperty, storages);
                }

                return (List<StorageModel>)this.GetValue(StoragesProperty);
            }
            set => this.SetValue(StoragesProperty, value);
        }

        public static readonly DependencyProperty StoragesProperty = DependencyProperty.Register(
            nameof(Storages), typeof(List<StorageModel>), typeof(DataFilterControl),
            new FrameworkPropertyMetadata(null, StoragesChangedCallback));

        private static void StoragesChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get filter
            var filter = (d as DataFilterControl);
            filter.ApplySelectedStorages();
        }

        #endregion

        #region Private methods (Categories)

        private bool isCatsApplying = false;

        private void ApplySelectedCategories()
        {
            if (!isCatsApplying && DataFilter != null)
            {
                isCatsApplying = true;
                bool allSelected = false;

                foreach (var cat in Categories.GetDescendants())
                {
                    // set selection
                    cat.IsSelected = DataFilter.CategoryIds.Count == 0 || DataFilter.CategoryIds.Contains(cat.Id);
                    // expand category in the tree if it's selected
                    if(cat.IsSelected) Categories.ExpandToDescendant(cat, true);
                    // add event on category selection changed
                    cat.PropertyChanged += (sender, e) =>
                    {
                        if(e.PropertyName == nameof(CategoryViewModel.IsSelected))
                        {
                            // get all categories
                            var allCats = Categories.GetDescendants().ToList();
                            // check all categories selected
                            allSelected = allCats.Count == allCats.Where(x => x.IsSelected).Count();

                            if (allSelected)
                            {
                                DataFilter.CategoryIds = new List<int>();
                                txtCatsCount.Text = MultiLangResourceManager.Instance[MultiLangResourceName.All];
                            }
                            else
                            {
                                DataFilter.CategoryIds = allCats.Where(x => x.IsSelected).Select(x => x.Id).ToList();
                                txtCatsCount.Text = DataFilter.CategoryIds.Count.ToString();
                            }
                        }
                    };
                }

                // get all categories
                var allcats = Categories.GetDescendants().ToList();
                // check all categories selected
                allSelected = allcats.Count == allcats.Where(x => x.IsSelected).Count();
                // expand categories expander if any category is selected
                CategoriesExpander.IsExpanded = DataFilter.CategoryIds.Count > 0;
                // populate selected categories count
                txtCatsCount.Text = allSelected
                    ? MultiLangResourceManager.Instance[MultiLangResourceName.All]
                    : DataFilter.CategoryIds.Count.ToString();
                isCatsApplying = false;
            }
        }

        #endregion

        #region Private methods (Storages)
        
        private bool isStoragesApplying = false;
        private List<SelectableTreeViewStorageGroupModel> storages;

        private void ApplySelectedStorages()
        {
            if (isStoragesApplying) return;
            if (DataFilter != null)
            {
                isStoragesApplying = true;
                bool allSelected = false;

                // build storage group list
                storages = Storages.Select(x => x.StorageGroupId).Distinct()
                    .Select(x => new SelectableTreeViewStorageGroupModel()
                    {
                        StorageGroup = Storages.First(s => s.StorageGroupId == x).StorageGroup
                    }).ToList();

                var storageTreeViewItems = new List<SelectableTreeViewItemModel<StorageModel>>();

                // fill storages tree view items for every storage group
                storages.ForEach(x =>
                {
                    // fill storages for every storage group
                    x.Storages = Storages.Where(s => s.StorageGroupId == x.StorageGroup.Id)
                        .Select(s =>
                        {
                            var storage = new SelectableTreeViewItemModel<StorageModel>
                            {
                                Value = s,
                                IsSelected = DataFilter.StorageIds.Count == 0 || DataFilter.StorageIds.Contains(s.Id)
                            };

                            storageTreeViewItems.Add(storage);

                            // add notification if selected storage list have been changed
                            storage.PropertyChanged += (sender, e) =>
                            {
                                if (e.PropertyName == nameof(SelectableTreeViewItemModel<StorageModel>.IsSelected))
                                {
                                    // check all storages selected
                                    allSelected = Storages.Count == storageTreeViewItems.Where(t => t.IsSelected).Count();

                                    if (allSelected)
                                    {
                                        DataFilter.StorageIds = new List<int>();
                                        txtStoragesCount.Text = MultiLangResourceManager.Instance[MultiLangResourceName.All];
                                    }
                                    else
                                    {
                                        DataFilter.StorageIds = storageTreeViewItems.Where(t => t.IsSelected).Select(p => p.Value.Id).ToList();
                                        txtStoragesCount.Text = DataFilter.StorageIds.Count.ToString();
                                    }
                                }
                            };

                            return storage;
                        }).ToList();

                    // expand branch if all storages are selected or any of this group
                    x.IsExpanded = DataFilter.StorageIds.Count == 0 || x.Storages.Any(s => s.IsSelected);
                });
                
                // check all storages selected
                allSelected = Storages.Count == storageTreeViewItems.Where(x => x.IsSelected).Count();
                // expand storages expander if any storage is selected
                StoragesExpander.IsExpanded = DataFilter.StorageIds.Count > 0;
                // populate selected categories count
                txtStoragesCount.Text = allSelected
                    ? MultiLangResourceManager.Instance[MultiLangResourceName.All]
                    : DataFilter.StorageIds.Count.ToString();
                isStoragesApplying = false;
                StoragesTree.ItemsSource = storages;
            }
        }

        #endregion

        #region Commands

        public ICommand ExpandAllCommand { get; set; }
        public ICommand CollapseAllCommand { get; set; }

        #endregion
    }
}
