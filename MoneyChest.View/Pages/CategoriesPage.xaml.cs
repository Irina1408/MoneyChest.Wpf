using MahApps.Metro.IconPacks;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Commands;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.View.ViewModel;
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

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : UserControl, IPage
    {
        #region Private fields

        private ICategoryService _service;
        private CategoriesPageViewModel _viewModel;
        private bool _reloadData = true;

        #endregion

        #region Initialization

        public CategoriesPage()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<CategoryService>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new CategoriesPageViewModel()
            {
                AddCommand = new Command(
                () => OpenDetails(new CategoryViewModel() { UserId = GlobalVariables.UserId }, true)),

                EditCommand = new TreeViewSelectedItemCommand<CategoryViewModel>(TreeViewCategories,
                (item) => OpenDetails(item)),

                DeleteCommand = new TreeViewSelectedItemCommand<CategoryViewModel>(TreeViewCategories,
                (item) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage(typeof(CategoryModel), new [] { item.Name });

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove in database
                        _service.Delete(item);
                        // remove in collection
                        _viewModel.Categories.RemoveDescendant(item);
                    }
                },
                (item) => item.Children.Count == 0),

                ChangeActivityCommand = new TreeViewSelectedItemCommand<CategoryViewModel>(TreeViewCategories,
                (item) =>
                {
                    // change activity
                    item.InHistory = !item.InHistory;
                    // save to database
                    _service.Update(item);
                    // update place in tree
                    ActivityChanged(item);
                })
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region IPage implementation

        public string Label => MultiLangResourceManager.Instance[MultiLangResourceName.Categories];
        public FrameworkElement Icon { get; private set; } = new PackIconEntypo() { Kind = PackIconEntypoKind.FlowTree };
        public int Order => 9;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion

        #region Event handlers

        private void CategoriesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_reloadData)
                ReloadData();
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (TreeViewCategories.SelectedItem != null)
            {
                _viewModel.EditCommand.Execute(TreeViewCategories.SelectedItem);
            }
        }

        private void TreeViewCategories_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeViewCategories.SelectedItem != null)
            {
                _viewModel.SelectedCategoryIsActive = !(TreeViewCategories.SelectedItem as CategoryViewModel).InHistory;
            }
        }

        #endregion

        #region Private methods

        private void ReloadData()
        {
            _viewModel.Categories = TreeHelper.BuildTree(_service.GetListForUser(GlobalVariables.UserId)
                .OrderBy(_ => _.InHistory)
                .ThenByDescending(_ => _.TransactionType)
                .ThenBy(_ => _.Name)
                .ToList());

            _reloadData = false;
        }

        private void OpenDetails(CategoryViewModel model, bool isNew = false)
        {
            // keep old parameters
            var oldParentId = model.ParentCategoryId;
            var oldInHistory = model.InHistory;
            var oldTransactionType = model.TransactionType;
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new CategoryDetailsView(_service, model, isNew, window.Close, _viewModel.Categories);
            // prepare window
            window.Height = 330;
            window.Width = 410;
            window.Content = detailsView;
            window.Closing += (sender, e) =>
            {
                if (!detailsView.CloseView())
                    e.Cancel = true;
            };
            // show window
            window.ShowDialog();
            if (detailsView.DialogResult)
            {
                // update tree
                if (isNew || model.ParentCategoryId != oldParentId)
                    Replace(model, oldParentId, isNew);
                else if (oldInHistory != model.InHistory)
                    ActivityChanged(model);

                // refrech background
                if(!isNew && oldTransactionType != model.TransactionType)
                    TreeViewCategories.Items.Refresh();

                // set expanded branch where category was changed
                _viewModel.Categories.ExpandMainViewToDescendant(model, true);
                model.IsSelectedMainView = true;
                // refresh commands
                RefreshCommandsState();
            }
        }

        private void Replace(CategoryViewModel model, int? oldParentId, bool isNew)
        {
            // remove entity from previous parent collection
            if (!isNew)
            {
                if (oldParentId.HasValue)
                {
                    var oldParent = _viewModel.Categories.GetDescendants().FirstOrDefault(_ => _.Id == oldParentId.Value);
                    oldParent.Children.Remove(model);
                }
                else
                    _viewModel.Categories.Remove(model);
            }

            if (model.ParentCategoryId.HasValue)
            {
                // find parent and add new child
                var parent = _viewModel.Categories.GetDescendants().FirstOrDefault(_ => _.Id == model.ParentCategoryId.Value);
                parent.Children.Add(model);
            }
            else
            {
                // get last in tree with the same transaction type and activity
                var lastInTree = _viewModel.Categories.LastOrDefault(_ => _.TransactionType == model.TransactionType && _.InHistory == model.InHistory);
                if (lastInTree != null)
                {
                    // insert after found item
                    _viewModel.Categories.Insert(_viewModel.Categories.IndexOf(lastInTree) + 1, model);
                }
                else
                {
                    // if it is income category insert into the start of collection
                    if (model.IsPlus) _viewModel.Categories.Insert(0, model);
                    // if it is expense category insert into the middle of collection
                    else if (model.IsMinus)
                    {
                        // get last income category
                        var firstIncome = _viewModel.Categories.LastOrDefault(_ => _.IsPlus);
                        if (firstIncome != null)
                        {
                            // insert after last income
                            _viewModel.Categories.Insert(_viewModel.Categories.IndexOf(firstIncome) + 1, model);
                        }
                        else
                            // else insert in start
                            _viewModel.Categories.Insert(0, model);
                    }
                    // if it for all types category insert it into the end of collection
                    else
                        _viewModel.Categories.Add(model);
                }
            }
        }

        private void ActivityChanged(CategoryViewModel model)
        {
            // replace
            if (model.ParentCategoryId.HasValue)
            {
                var parent = _viewModel.Categories.GetDescendants().FirstOrDefault(_ => _.Id == model.ParentCategoryId.Value);
                parent.Children.Move(parent.Children.IndexOf(model), model.InHistory ? parent.Children.Count - 1 : 0);
            }
            else
                _viewModel.Categories.Move(_viewModel.Categories.IndexOf(model), model.InHistory ? _viewModel.Categories.Count - 1 : 0);

            // update children activity
            foreach (var child in model.Children)
                child.InHistory = model.InHistory;
        }

        private void RefreshCommandsState()
        {
            _viewModel.EditCommand.ValidateCanExecute();
            _viewModel.DeleteCommand.ValidateCanExecute();
            _viewModel.ChangeActivityCommand.ValidateCanExecute();

            if (TreeViewCategories.SelectedItem != null)
            {
                _viewModel.SelectedCategoryIsActive = !(TreeViewCategories.SelectedItem as CategoryViewModel).InHistory;
            }
        }

        #endregion
    }
}
