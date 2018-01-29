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
    /// Interaction logic for PlanningPage.xaml
    /// </summary>
    public partial class PlanningPage : UserControl, IPage
    {
        #region Private fields

        private IMoneyTransferEventService _moneyTransferEventService;
        private IRepayDebtEventService _repayDebtEventService;
        private ISimpleEventService _simpleEventService;
        private PlanningPageViewModel _viewModel;
        // TODO: replace to IPage Options
        private bool _reload = true;

        #endregion

        #region Initialization

        public PlanningPage()
        {
            InitializeComponent();

            // init
            _moneyTransferEventService = ServiceManager.ConfigureService<MoneyTransferEventService>();
            _repayDebtEventService = ServiceManager.ConfigureService<RepayDebtEventService>();
            _simpleEventService = ServiceManager.ConfigureService<SimpleEventService>();
            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = new PlanningPageViewModel()
            {
                SimpleEventsViewModel = new EventListViewModel<SimpleEventViewModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new SimpleEventViewModel() { UserId = GlobalVariables.UserId, Schedule = new ScheduleModel() }, true)),

                    EditCommand = new DataGridSelectedItemCommand<SimpleEventViewModel>(GridSimpleEvents,
                    (item) => OpenDetails(item)),

                    DeleteCommand = new DataGridSelectedItemsCommand<SimpleEventViewModel>(GridSimpleEvents,
                    (items) =>
                    {
                        var message = MultiLangResource.DeletionConfirmationMessage(typeof(SimpleEventModel), items.Select(_ => _.Description));

                        if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            // remove in database
                            _simpleEventService.Delete(items);
                            // remove in grid
                            foreach (var item in items.ToList())
                                _viewModel.SimpleEventsViewModel.Events.Remove(item);
                        }
                    })
                },

                MoneyTransferEventsViewModel = new EventListViewModel<MoneyTransferEventViewModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new MoneyTransferEventViewModel() { UserId = GlobalVariables.UserId }, true)),

                    EditCommand = new DataGridSelectedItemCommand<MoneyTransferEventViewModel>(GridMoneyTransferEvents,
                    (item) => OpenDetails(item)),

                    DeleteCommand = new DataGridSelectedItemsCommand<MoneyTransferEventViewModel>(GridMoneyTransferEvents,
                    (items) =>
                    {
                        var message = MultiLangResource.DeletionConfirmationMessage(typeof(MoneyTransferEventModel), items.Select(_ => _.Description));

                        if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            // remove in database
                            _moneyTransferEventService.Delete(items);
                            // remove in grid
                            foreach (var item in items.ToList())
                                _viewModel.MoneyTransferEventsViewModel.Events.Remove(item);
                        }
                    })
                },

                RepayDebtEventsViewModel = new EventListViewModel<RepayDebtEventViewModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new RepayDebtEventViewModel() { UserId = GlobalVariables.UserId }, true)),

                    EditCommand = new DataGridSelectedItemCommand<RepayDebtEventViewModel>(GridRepayDebtEvents,
                    (item) => OpenDetails(item)),

                    DeleteCommand = new DataGridSelectedItemsCommand<RepayDebtEventViewModel>(GridRepayDebtEvents,
                    (items) =>
                    {
                        var message = MultiLangResource.DeletionConfirmationMessage(typeof(RepayDebtEventModel), items.Select(_ => _.Description));

                        if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            // remove in database
                            _repayDebtEventService.Delete(items);
                            // remove in grid
                            foreach (var item in items.ToList())
                                _viewModel.RepayDebtEventsViewModel.Events.Remove(item);
                        }
                    })
                }
            };

            SimpleEventsBorder.DataContext = _viewModel.SimpleEventsViewModel;
            MoneyTransferEventsBorder.DataContext = _viewModel.MoneyTransferEventsViewModel;
            RepayDebtEventsBorder.DataContext = _viewModel.RepayDebtEventsViewModel;
        }

        #endregion

        #region IPage implementation

        public string Label => MultiLangResourceManager.Instance[MultiLangResourceName.Planning];
        public FrameworkElement Icon { get; private set; } = new PackIconMaterial() { Kind = PackIconMaterialKind.CalendarClock };
        public int Order => 4;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion

        #region Event handlers

        private void PlanningPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (_reload)
                ReloadData();
        }

        private void EventsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender == GridSimpleEvents)
                _viewModel.SimpleEventsViewModel.EditCommand.Execute(GridSimpleEvents.SelectedItem);
            else if (sender == GridMoneyTransferEvents)
                _viewModel.MoneyTransferEventsViewModel.EditCommand.Execute(GridMoneyTransferEvents.SelectedItem);
            else if (sender == GridRepayDebtEvents)
                _viewModel.RepayDebtEventsViewModel.EditCommand.Execute(GridRepayDebtEvents.SelectedItem);
        }

        #endregion

        #region Private methods

        private void ReloadData()
        {
            // reload Simple events
            _viewModel.SimpleEventsViewModel.Events = new ObservableCollection<SimpleEventViewModel>(
                _simpleEventService.GetListForUser(GlobalVariables.UserId)
                .Select(e => new SimpleEventViewModel(e))
                .OrderBy(_ => _.EventState));

            // reload Money Transfer Events
            _viewModel.MoneyTransferEventsViewModel.Events = new ObservableCollection<MoneyTransferEventViewModel>(
                _moneyTransferEventService.GetListForUser(GlobalVariables.UserId)
                .Select(e => new MoneyTransferEventViewModel(e))
                .OrderBy(_ => _.EventState));

            // reload Repay Debt Events 
            _viewModel.RepayDebtEventsViewModel.Events = new ObservableCollection<RepayDebtEventViewModel>(
                _repayDebtEventService.GetListForUser(GlobalVariables.UserId)
                .Select(e => new RepayDebtEventViewModel(e))
                .OrderBy(_ => _.EventState));

            GridSimpleEvents.ItemsSource = _viewModel.SimpleEventsViewModel.Events;
            GridMoneyTransferEvents.ItemsSource = _viewModel.MoneyTransferEventsViewModel.Events;
            GridRepayDebtEvents.ItemsSource = _viewModel.RepayDebtEventsViewModel.Events;
            // mark as reloaded
            //_reload = false;
        }

        private void OpenDetails(SimpleEventViewModel model, bool isNew = false)
        {
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new SimpleEventDetailsView(_simpleEventService, model, isNew, window.Close);
            // prepare window
            window.Height = 650;
            window.Width = 830;
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
                    InsertNewEvent(_viewModel.SimpleEventsViewModel.Events, model);

                GridSimpleEvents.Items.Refresh();
            }
        }

        private void OpenDetails(MoneyTransferEventViewModel model, bool isNew = false)
        {
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new MoneyTransferEventDetailsView(_moneyTransferEventService, model, isNew, window.Close);
            // prepare window
            window.Height = 600;
            window.Width = 770;
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
                    InsertNewEvent(_viewModel.MoneyTransferEventsViewModel.Events, model);

                GridMoneyTransferEvents.Items.Refresh();
            }
        }

        private void OpenDetails(RepayDebtEventViewModel model, bool isNew = false)
        {
            // init window and details view
            var window = this.InitializeDependWindow(false);
            var detailsView = new RepayDebtEventDetailsView(_repayDebtEventService, model, isNew, window.Close);
            // prepare window
            window.Height = 600;
            window.Width = 780;
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
                    InsertNewEvent(_viewModel.RepayDebtEventsViewModel.Events, model);

                GridRepayDebtEvents.Items.Refresh();
            }
        }

        private void InsertNewEvent<T>(ObservableCollection<T> events, T model)
            where T : EventModel
        {
            // insert new event
            if (events.Any(x => x.EventState != Model.Enums.EventState.Active))
            {
                var firstNonActive = events.First(x => x.EventState != Model.Enums.EventState.Active);
                if (events.IndexOf(firstNonActive) > 0)
                    events.Insert(events.IndexOf(firstNonActive), model);
                else
                    events.Insert(0, model);
            }
            else
                events.Add(model);
        }

        #endregion
    }
}
