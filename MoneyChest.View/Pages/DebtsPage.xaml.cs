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
    public partial class DebtsPage : PageBase
    {
        #region Private fields

        private IDebtService _service;
        private EntityListViewModel<DebtViewModel> _viewModel;

        #endregion

        #region Initialization

        public DebtsPage() : base()
        {
            InitializeComponent();

            // init
            _service = ServiceManager.ConfigureService<DebtService>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new EntityListViewModel<DebtViewModel>()
            {
                AddCommand = new Command(
                () => OpenDetails(_service.PrepareNew(new DebtViewModel() { UserId = GlobalVariables.UserId }) as DebtViewModel, true)),

                EditCommand = new DataGridSelectedItemCommand<DebtViewModel>(GridDebts,
                (item) => OpenDetails(item), null, true),

                DeleteCommand = new DataGridSelectedItemsCommand<DebtViewModel>(GridDebts,
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
                            _viewModel.Entities.Remove(item);
                        NotifyDataChanged();
                    }
                })
            };

            this.DataContext = _viewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            // reload debts
            _viewModel.Entities = new ObservableCollection<DebtViewModel>(
                _service.GetListForUser(GlobalVariables.UserId)
                .Select(e => new DebtViewModel(e))
                .OrderBy(_ => _.IsRepaid)
                .ThenByDescending(_ => _.TakingDate));
        }

        #endregion

        #region Private methods

        private void OpenDetails(DebtViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new DebtDetailsView(_service, model, isNew), () =>
            {
                // update grid
                if (isNew)
                {
                    // insert new debt
                    if (model.IsRepaid)
                        _viewModel.Entities.Add(model);
                    else
                        _viewModel.Entities.Insert(0, model);
                }

                GridDebts.Items.Refresh();
                NotifyDataChanged();
            });
        }

        #endregion
    }
}
