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
    /// Interaction logic for DebtsPage.xaml
    /// </summary>
    public partial class DebtsPage : UserControl, IPage
    {
        #region Private fields

        private IDebtService _service;
        private ICurrencyExchangeRateService _currencyExchangeRateService;
        private DebtsPageViewModel _viewModel;
        // TODO: replace to IPage Options
        private bool _reload = true;

        #endregion

        #region Initialization

        public DebtsPage()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<DebtService>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new DebtsPageViewModel()
            {
                AddCommand = new Command(
                () => OpenDetails(new DebtModel() { UserId = GlobalVariables.UserId }, true)),

                EditCommand = new DataGridSelectedItemCommand<DebtModel>(GridDebts,
                (item) => OpenDetails(item)),

                DeleteCommand = new DataGridSelectedItemsCommand<DebtModel>(GridDebts,
                (items) =>
                {
                    var message = MultiLangResource.DeletionConfirmationMessage(typeof(DebtModel), items.Select(_ => _.Description));

                    if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                        MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        // remove in database
                        _service.Delete(items);
                        // remove in grid
                        foreach (var item in items.ToList())
                            _viewModel.Debts.Remove(item);
                    }
                })
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region IPage implementation

        public string Label => MultiLangResourceManager.Instance[MultiLangResourceName.Debts];
        public FrameworkElement Icon { get; private set; } = new PackIconModern() { Kind = PackIconModernKind.CalendarDollar };
        public int Order => 5;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion

        #region Event handlers

        private void DebtsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_reload)
                ReloadData();
        }

        private void GridDebts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GridDebts.SelectedItem != null)
            {
                _viewModel.EditCommand.Execute(GridDebts.SelectedItem);
            }
        }

        #endregion

        #region Private methods

        private void ReloadData()
        {
            // reload debts
            _viewModel.Debts = new ObservableCollection<DebtModel>(
                _service.GetListForUser(GlobalVariables.UserId)
                .OrderBy(_ => _.IsRepaid)
                .ThenByDescending(_ => _.TakingDate));

            // mark as reloaded
            _reload = false;
        }

        private void OpenDetails(DebtModel model, bool isNew = false)
        {
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new DebtDetailsView(_service, model, isNew, window.Close);
            // prepare window
            window.Height = 572;
            window.Width = 1110;
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
                    // insert new debt
                    if (model.IsRepaid)
                        _viewModel.Debts.Add(model);
                    else
                        _viewModel.Debts.Insert(0, model);
                }

                GridDebts.Items.Refresh();
            }
        }

        #endregion
    }
}
