using MahApps.Metro.IconPacks;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.ViewModel;
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
    public partial class CategoriesPage : PageBase
    {
        #region Private fields

        private ICategoryService _service;
        private CategoriesPageViewModel _viewModel;

        #endregion

        #region Initialization

        public CategoriesPage() : base()
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
                (item) => OpenDetails(item), null, true),

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
                        NotifyDataChanged();
                    }
                },
                (item) => item.Children.Count == 0),

                ChangeActivityCommand = new TreeViewSelectedItemCommand<CategoryViewModel>(TreeViewCategories,
                (item) =>
                {
                    // change activity
                    item.IsActive = !item.IsActive;
                    // save to database
                    _service.Update(item);
                    // reload data
                    Reload();
                    NotifyDataChanged();
                })
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            var oldCategoryCollection = _viewModel.Categories?.GetDescendants();

            _viewModel.Categories = TreeHelper.BuildTree(_service.GetListForUser(GlobalVariables.UserId)
                .OrderByDescending(_ => _.IsActive)
                .ThenByDescending(_ => _.RecordType)
                .ThenBy(_ => _.Name)
                .ToList());

            // update selected and expanded items
            if (oldCategoryCollection != null)
            {
                foreach (var cat in _viewModel.Categories.GetDescendants())
                {
                    var old = oldCategoryCollection.FirstOrDefault(_ => _.Id == cat.Id);
                    if (old != null)
                    {
                        cat.IsExpandedMainView = old.IsExpandedMainView;
                        cat.IsSelectedMainView = old.IsSelectedMainView;
                    }
                }
            }
        }

        #endregion

        #region Event handlers

        private void TreeViewCategories_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (TreeViewCategories.SelectedItem != null)
            {
                _viewModel.SelectedCategoryIsActive = (TreeViewCategories.SelectedItem as CategoryViewModel).IsActive;
            }
        }

        #endregion

        #region Private methods

        private void OpenDetails(CategoryViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new CategoryDetailsView(_service, model, isNew, _viewModel.Categories), () =>
            {
                // set expanded branch where category was changed
                _viewModel.Categories.ExpandMainViewToDescendant(model, true);
                model.IsSelectedMainView = true;

                // reload data
                Reload();
                // refresh commands
                RefreshCommandsState();
                NotifyDataChanged();
            });
        }

        private void RefreshCommandsState()
        {
            _viewModel.EditCommand.ValidateCanExecute();
            _viewModel.DeleteCommand.ValidateCanExecute();
            _viewModel.ChangeActivityCommand.ValidateCanExecute();

            if (TreeViewCategories.SelectedItem != null)
            {
                _viewModel.SelectedCategoryIsActive = !(TreeViewCategories.SelectedItem as CategoryViewModel).IsActive;
            }
        }

        #endregion
    }
}
