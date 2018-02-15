﻿using MahApps.Metro.IconPacks;
using MoneyChest.Model.Enums;
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
    public partial class PlanningPage : PageBase
    {
        #region Private fields

        private IMoneyTransferEventService _moneyTransferEventService;
        private IRepayDebtEventService _repayDebtEventService;
        private ISimpleEventService _simpleEventService;
        private ILimitService _limitService;
        private PlanningPageViewModel _viewModel;
        private bool _areLimitsLoaded;

        #endregion

        #region Initialization

        public PlanningPage() : base()
        {
            InitializeComponent();

            // init
            _moneyTransferEventService = ServiceManager.ConfigureService<MoneyTransferEventService>();
            _repayDebtEventService = ServiceManager.ConfigureService<RepayDebtEventService>();
            _simpleEventService = ServiceManager.ConfigureService<SimpleEventService>();
            _limitService = ServiceManager.ConfigureService<LimitService>();
            InitializeViewModel();
            _areLimitsLoaded = false;
        }

        private void InitializeViewModel()
        {
            _viewModel = new PlanningPageViewModel()
            {
                SimpleEventsViewModel = new EntityListViewModel<SimpleEventViewModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new SimpleEventViewModel() { UserId = GlobalVariables.UserId, Schedule = new ScheduleModel() }, true)),

                    EditCommand = new DataGridSelectedItemCommand<SimpleEventViewModel>(GridSimpleEvents,
                    (item) => OpenDetails(item), null, true),

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
                                _viewModel.SimpleEventsViewModel.Entities.Remove(item);

                            NotifyDataChanged();
                        }
                    })
                },

                MoneyTransferEventsViewModel = new EntityListViewModel<MoneyTransferEventViewModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new MoneyTransferEventViewModel() { UserId = GlobalVariables.UserId }, true)),

                    EditCommand = new DataGridSelectedItemCommand<MoneyTransferEventViewModel>(GridMoneyTransferEvents,
                    (item) => OpenDetails(item), null, true),

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
                                _viewModel.MoneyTransferEventsViewModel.Entities.Remove(item);

                            NotifyDataChanged();
                        }
                    })
                },

                RepayDebtEventsViewModel = new EntityListViewModel<RepayDebtEventViewModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new RepayDebtEventViewModel() { UserId = GlobalVariables.UserId }, true)),

                    EditCommand = new DataGridSelectedItemCommand<RepayDebtEventViewModel>(GridRepayDebtEvents,
                    (item) => OpenDetails(item), null, true),

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
                                _viewModel.RepayDebtEventsViewModel.Entities.Remove(item);

                            NotifyDataChanged();
                        }
                    })
                },

                LimitsViewModel = new EntityListViewModel<LimitModel>()
                {
                    AddCommand = new Command(
                        () => OpenDetails(new LimitModel() { UserId = GlobalVariables.UserId }, true)),

                    EditCommand = new DataGridSelectedItemCommand<LimitModel>(GridLimits,
                    (item) => OpenDetails(item), null, true),

                    DeleteCommand = new DataGridSelectedItemsCommand<LimitModel>(GridLimits,
                    (items) =>
                    {
                        var message = MultiLangResource.DeletionConfirmationMessage(typeof(LimitModel), null);

                        if (MessageBox.Show(message, MultiLangResourceManager.Instance[MultiLangResourceName.DeletionConfirmation],
                            MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                        {
                            // remove in database
                            _limitService.Delete(items);
                            // remove in grid
                            foreach (var item in items.ToList())
                                _viewModel.LimitsViewModel.Entities.Remove(item);

                            NotifyDataChanged();
                        }
                    })
                }
            };

            //LimitStateColumn.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(LimitState));
            SimpleEventsBorder.DataContext = _viewModel.SimpleEventsViewModel;
            MoneyTransferEventsBorder.DataContext = _viewModel.MoneyTransferEventsViewModel;
            RepayDebtEventsBorder.DataContext = _viewModel.RepayDebtEventsViewModel;
            ExpanderLimits.DataContext = _viewModel.LimitsViewModel;
        }

        #endregion

        #region Overrides

        public override void Reload()
        {
            base.Reload();

            // reload Simple events
            _viewModel.SimpleEventsViewModel.Entities = new ObservableCollection<SimpleEventViewModel>(
                _simpleEventService.GetListForUser(GlobalVariables.UserId)
                .Select(e => new SimpleEventViewModel(e))
                .OrderBy(_ => _.EventState));

            // reload Money Transfer Events
            _viewModel.MoneyTransferEventsViewModel.Entities = new ObservableCollection<MoneyTransferEventViewModel>(
                _moneyTransferEventService.GetListForUser(GlobalVariables.UserId)
                .Select(e => new MoneyTransferEventViewModel(e))
                .OrderBy(_ => _.EventState));

            // reload Repay Debt Events 
            _viewModel.RepayDebtEventsViewModel.Entities = new ObservableCollection<RepayDebtEventViewModel>(
                _repayDebtEventService.GetListForUser(GlobalVariables.UserId)
                .Select(e => new RepayDebtEventViewModel(e))
                .OrderBy(_ => _.EventState));

            GridSimpleEvents.ItemsSource = _viewModel.SimpleEventsViewModel.Entities;
            GridMoneyTransferEvents.ItemsSource = _viewModel.MoneyTransferEventsViewModel.Entities;
            GridRepayDebtEvents.ItemsSource = _viewModel.RepayDebtEventsViewModel.Entities;

            // reload limits
            if (ExpanderLimits.IsExpanded)
                LoadLimits();
            else
                _areLimitsLoaded = false;
        }

        #endregion

        #region Event handlers
        
        private void ExpanderLimits_Expanded(object sender, RoutedEventArgs e)
        {
            if (!_areLimitsLoaded)
                LoadLimits();
        }

        #endregion

        #region Private methods

        private void OpenDetails(SimpleEventViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new SimpleEventDetailsView(_simpleEventService, model, isNew), () =>
            {
                // update grid
                if (isNew)
                    InsertNewEvent(_viewModel.SimpleEventsViewModel.Entities, model);

                GridSimpleEvents.Items.Refresh();
                NotifyDataChanged();
            });
        }

        private void OpenDetails(MoneyTransferEventViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new MoneyTransferEventDetailsView(_moneyTransferEventService, model, isNew), () =>
            {
                // update grid
                if (isNew)
                    InsertNewEvent(_viewModel.MoneyTransferEventsViewModel.Entities, model);

                GridMoneyTransferEvents.Items.Refresh();
                NotifyDataChanged();
            });
        }

        private void OpenDetails(RepayDebtEventViewModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new RepayDebtEventDetailsView(_repayDebtEventService, model, isNew), () =>
            {
                // update grid
                if (isNew)
                    InsertNewEvent(_viewModel.RepayDebtEventsViewModel.Entities, model);

                GridRepayDebtEvents.Items.Refresh();
                NotifyDataChanged();
            });
        }

        private void OpenDetails(LimitModel model, bool isNew = false)
        {
            this.OpenDetailsWindow(new LimitDetailsView(_limitService, model, isNew), () =>
            {
                // update grid
                if (isNew)
                    InsertNewLimit(_viewModel.LimitsViewModel.Entities, model);

                GridLimits.Items.Refresh();
                NotifyDataChanged();
            });
        }

        private void InsertNewLimit(ObservableCollection<LimitModel> entities, LimitModel model)
        {
            // insert new limit
            if(model.State == LimitState.Planned && entities.Any(x => x.State != Model.Enums.LimitState.Active))
            {
                var firstNonActive = entities.First(x => x.State != LimitState.Active);
                if (entities.IndexOf(firstNonActive) > 0)
                    entities.Insert(entities.IndexOf(firstNonActive), model);
                else
                    entities.Insert(0, model);
            }
            else if (model.State == LimitState.Active)
                entities.Insert(0, model);
            else
                entities.Add(model);
        }

        private void InsertNewEvent<T>(ObservableCollection<T> events, T model)
            where T : EventModel
        {
            // insert new event
            if (events.Any(x => x.EventState != EventState.Active))
            {
                var firstNonActive = events.First(x => x.EventState != EventState.Active);
                if (events.IndexOf(firstNonActive) > 0)
                    events.Insert(events.IndexOf(firstNonActive), model);
                else
                    events.Insert(0, model);
            }
            else
                events.Add(model);
        }

        private void LoadLimits()
        {
            _viewModel.LimitsViewModel.Entities = new ObservableCollection<LimitModel>(
                _limitService.GetListForUser(GlobalVariables.UserId).OrderBy(_ => _.State));

            GridLimits.ItemsSource = _viewModel.LimitsViewModel.Entities;
            _areLimitsLoaded = true;
        }

        #endregion
    }
}
