using MaterialDesignThemes.Wpf;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Commands;
using MoneyChest.View.ViewModel;
using MoneyChest.View.Wrappers;
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
    /// <summary>
    /// Interaction logic for CategoryDetailsView.xaml
    /// </summary>
    public partial class CategoryDetailsView : UserControl
    {
        #region Private fields

        private ICategoryService _service;
        private EntityWrapper<CategoryModel> _wrappedEntity;
        private bool _isNew;
        private DetailsViewCommandContainer _commands;
        private Action _closeAction;
        private bool _closeView;
        private CategoryViewModelCollection _categories;

        #endregion

        #region Initialization

        public CategoryDetailsView(ICategoryService service, CategoryModel entity, bool isNew, Action closeAction,
            CategoryViewModelCollection categories)
        {
            InitializeComponent();

            // init
            _service = service;
            _isNew = isNew;
            _closeAction = closeAction;
            _categories = new CategoryViewModelCollection(categories);

            // add empty category
            _categories.Insert(0, new CategoryViewModel()
            {
                Id = -1,
                Name = MultiLangResourceManager.Instance[MultiLangResourceName.None]
            });

            // set defaults
            _closeView = false;
            LabelHeader.Content = isNew
                ? MultiLangResourceManager.Instance[MultiLangResourceName.New(typeof(CategoryModel))]
                : MultiLangResourceManager.Instance[MultiLangResourceName.Singular(typeof(CategoryModel))];
            
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

            // initialize datacontexts
            _wrappedEntity = new EntityWrapper<CategoryModel>(entity);
            this.DataContext = _wrappedEntity.Entity;
            TreeViewCategories.ItemsSource = _categories;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            _commands = new DetailsViewCommandContainer()
            {
                SaveCommand = new Command(() =>
                {
                    // save changes
                    SaveChanges();
                    // close control
                    _closeAction?.Invoke();
                },
                () => _wrappedEntity.IsChanged && !_wrappedEntity.HasErrors),

                CancelCommand = new Command(() =>
                {
                    if (CloseView())
                        _closeAction?.Invoke();
                })
            };

            // add events
            _wrappedEntity.Entity.PropertyChanged += (sender, args) => ((Command)_commands.SaveCommand).ValidateCanExecute();
            // validate save command now 
            _commands.SaveCommand.ValidateCanExecute();

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

        #region Public

        public bool DialogResult { get; private set; } = false;

        public void SaveChanges()
        {
            if (_isNew)
                _service.Add(_wrappedEntity.Entity);
            else
                _service.Update(_wrappedEntity.Entity);

            DialogResult = true;
            _closeView = true;
        }

        public void RevertChanges()
        {
            _wrappedEntity.RevertChanges();

            DialogResult = false;
            _closeView = true;
        }

        public bool CloseView()
        {
            // not ask confirmation if it has already asked
            if (_closeView) return _closeView;

            // ask confirmation only if any changes exists
            if (_wrappedEntity.IsChanged)
            {
                // show confirmation
                var dialogResult = MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmationMessage], MultiLangResourceManager.Instance[MultiLangResourceName.SaveChangesConfirmation], MessageBoxButton.YesNoCancel, MessageBoxImage.Exclamation, MessageBoxResult.Yes);

                if (dialogResult == MessageBoxResult.Yes)
                {
                    // check errors
                    if (_wrappedEntity.HasErrors)
                        MessageBox.Show(MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailedMessage], MultiLangResourceManager.Instance[MultiLangResourceName.SaveFailed], MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    else
                        SaveChanges();
                }
                else if (dialogResult == MessageBoxResult.No)
                    RevertChanges();
            }
            else
                _closeView = true;

            return _closeView;
        }

        #endregion
    }
}
