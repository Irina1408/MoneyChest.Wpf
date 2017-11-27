using MahApps.Metro.IconPacks;
using MoneyChest.Model.Model;
using MoneyChest.Services;
using MoneyChest.Services.Services;
using MoneyChest.Shared;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Details;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for StoragesPage.xaml
    /// </summary>
    public partial class StoragesPage : UserControl, IPage
    {
        #region Private fields

        private IStorageService _service;
        private IStorageGroupService _storageGroupService;
        private ICurrencyService _currencyService;

        private StoragesPageViewModel _viewModel;

        private List<StorageViewModel> _storages;
        private List<StorageGroupViewModel> _storageGroups;
        private List<CurrencyModel> _currencies;

        // view data
        private Dictionary<int, WrapPanel> _storageGroupPanel;
        private Dictionary<int, ContentControl> _storageView;

        // TODO: replace to IPage Options
        private bool _reload = true;

        #endregion

        #region Initialization

        public StoragesPage()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<StorageService>();
            _storageGroupService = ServiceManager.ConfigureService<StorageGroupService>();
            _currencyService = ServiceManager.ConfigureService<CurrencyService>();
            _storageGroupPanel = new Dictionary<int, WrapPanel>();
            _storageView = new Dictionary<int, ContentControl>();
            _storages = new List<StorageViewModel>();
            _storageGroups = new List<StorageGroupViewModel>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new StoragesPageViewModel()
            {
                AddStorageCommand = new ParametrizedCommand<StorageGroupViewModel>(
                    (item) => OpenDetails(new StorageViewModel()
                    {
                        UserId = GlobalVariables.UserId,
                        StorageGroupId = item.Id,
                        CurrencyId = _currencies.FirstOrDefault(_ => _.IsMain).Id
                    }, true)),

                EditStorageCommand = new ParametrizedCommand<StorageViewModel>(
                    (item) => OpenDetails(item)),

                DeleteStorageCommand = new ParametrizedCommand<StorageViewModel>(
                    (item) =>
                    {
                        var message = MultiLangResource.DeletionConfirmationMessage(typeof(StorageModel), new[] { item.Name });

                        if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            // remove in database
                            _service.Delete(item);
                            // remove in view
                            _storageGroupPanel[item.StorageGroupId].Children.Remove(_storageView[item.Id]);
                        }
                    }),

                AddStorageGroupCommand = new Command(
                    () => 
                    {
                        // create storage group
                        var storageGroup = new StorageGroupModel()
                        {
                            UserId = GlobalVariables.UserId
                        };
                        // add storage group into database
                        _storageGroupService.Add(storageGroup);
                        // add storage group into view
                        AddStorageGroupIntoView(storageGroup);
                        // scroll down
                        StoragesScrollViewer.ScrollToEnd();
                    }),

                DeleteStorageGroupCommand = new ParametrizedCommand<StorageGroupViewModel>(
                    (item) =>
                    {
                        var message = MultiLangResource.DeletionConfirmationMessage(typeof(StorageGroupModel), new[] { item.Name });

                        if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            // remove in database
                            _storageGroupService.Delete(item);
                            // update list
                            _storageGroups.Remove(_storageGroups.First(_ => _.Id == item.Id));
                            // remove in view
                            StoragesPanel.Children.Remove(_storageGroupPanel[item.Id].Parent as ContentControl);
                        }
                    })
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region IPage implementation

        public string Label => MultiLangResourceManager.Instance[MultiLangResourceName.Storages];
        public FrameworkElement Icon { get; private set; } = new PackIconMaterial() { Kind = PackIconMaterialKind.Bank };
        public int Order => 7;
        public bool ShowTopBorder => true;
        public FrameworkElement View => this;

        #endregion

        #region Event handlers

        private void StoragesPage_Loaded(object sender, RoutedEventArgs e)
        {
            if(_reload)
                ReloadData();
        }

        private void StorageGroupName_LostFocus(object sender, RoutedEventArgs e)
        {
            // get changed storage group
            var textBox = sender as TextBox;
            var storageGroupViewModel = textBox.DataContext as StorageGroupViewModel;
            // save changes to database
            if(storageGroupViewModel.IsChanged)
            {
                _storageGroupService.Update(storageGroupViewModel);
                storageGroupViewModel.IsChanged = false;
            }
        }

        private void StorageVisibilityToggleButton_Click(object sender, RoutedEventArgs e)
        {
            // get changed storage
            var toggleButton = sender as ToggleButton;
            var storageViewModel = toggleButton.DataContext as StorageViewModel;
            // save changes
            _service.Update(storageViewModel);
        }

        #endregion

        #region Private methods

        private void ReloadData()
        {
            // reload storages
            var storages = _service.GetListForUser(GlobalVariables.UserId);
            var storageGroups = _storageGroupService.GetListForUser(GlobalVariables.UserId);
            _currencies = _currencyService.GetActive(GlobalVariables.UserId);

            // cleanup
            StoragesPanel.Children.Clear();
            _storageGroupPanel.Clear();
            _storageView.Clear();

            // fill grid
            foreach (var storageGroup in storageGroups)
            {
                AddStorageGroupIntoView(storageGroup, storages);
            }

            // mark as reloaded
            _reload = false;
        }

        private void AddStorageGroupIntoView(StorageGroupModel storageGroup, List<StorageModel> storages = null)
        {
            // create panel for storages
            _storageGroupPanel.Add(storageGroup.Id, new WrapPanel());

            if(storages != null)
            {
                foreach (var currency in _currencies.OrderByDescending(_ => _.IsMain))
                {
                    // add storages into panel
                    foreach (var storage in storages.Where(_ => _.StorageGroupId == storageGroup.Id && _.CurrencyId == currency.Id))
                    {
                        AddStorageIntoView(storage);
                    }
                }
            }

            var storageGroupViewModel = new StorageGroupViewModel(storageGroup)
            {
                AddStorageCommand = _viewModel.AddStorageCommand,
                DeleteCommand = _viewModel.DeleteStorageGroupCommand
            };

            // create control for storage group data
            var storageGroupControl = new ContentControl()
            {
                Template = this.Resources["StorageGroupControlTemplate"] as ControlTemplate,
                Content = _storageGroupPanel[storageGroup.Id],
                DataContext = storageGroupViewModel
            };

            _storageGroups.Add(storageGroupViewModel);
            StoragesPanel.Children.Add(storageGroupControl);
        }

        private void AddStorageIntoView(StorageModel storage)
        {
            var storageViewModel = new StorageViewModel(storage)
            {
                EditCommand = _viewModel.EditStorageCommand,
                DeleteCommand = _viewModel.DeleteStorageCommand
            };

            var itemView = new ContentControl()
            {
                Template = this.Resources["StorageItemControlTemplate"] as ControlTemplate,
                DataContext = storageViewModel
            };

            _storages.Add(storageViewModel);
            _storageGroupPanel[storage.StorageGroupId].Children.Add(itemView);
            _storageView.Add(storage.Id, itemView);
        }

        private void RefreshStoragesSequence(int storageGroupId)
        {
            // get storages of storage group
            var storages = _storages.Where(_ => _.StorageGroupId == storageGroupId);
            if (storages.Select(_ => _.CurrencyId).Distinct().Count() <= 1) return;
            // clear panel
            _storageGroupPanel[storageGroupId].Children.Clear();
            // refill storage group panel
            foreach (var currency in _currencies.OrderByDescending(_ => _.IsMain))
            {
                foreach (var storage in storages.Where(_ => _.CurrencyId == currency.Id))
                    _storageGroupPanel[storageGroupId].Children.Add(_storageView[storage.Id]);
            }
        }

        private void OpenDetails(StorageViewModel model, bool isNew = false)
        {
            // keep storage
            var oldStorageGroupId = model.StorageGroupId;
            var oldCurrencyId = model.CurrencyId;
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new StorageDetailsView(_service, model, isNew, window.Close, _storageGroups.Cast<StorageGroupModel>(), _currencies);
            // prepare window
            window.Height = 440;
            window.Width = 270;
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
                // update grid
                if (isNew)
                {
                    // insert new storage into view
                    AddStorageIntoView(model);
                }
                else if(oldStorageGroupId != model.StorageGroupId)
                {
                    // remove from old group
                    _storageGroupPanel[oldStorageGroupId].Children.Remove(_storageView[model.Id]);
                    // add into new group
                    _storageGroupPanel[model.StorageGroupId].Children.Add(_storageView[model.Id]);
                    RefreshStoragesSequence(model.StorageGroupId);
                }
                else if(oldCurrencyId != model.CurrencyId)
                    RefreshStoragesSequence(model.StorageGroupId);
            }
        }

        #endregion
    }
}
