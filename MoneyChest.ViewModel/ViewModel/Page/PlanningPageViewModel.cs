using MoneyChest.Model.Model;
using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    public class PlanningPageViewModel
    {
        public PlanningPageViewModel()
        {
            SimpleEventsViewModel = new EventListViewModel<SimpleEventViewModel>();
            MoneyTransferEventsViewModel = new EventListViewModel<MoneyTransferEventViewModel>();
            RepayDebtEventsViewModel = new EventListViewModel<RepayDebtEventViewModel>();
            LimitsViewModel = new LimitListViewModel<LimitModel>();
        }

        public EventListViewModel<SimpleEventViewModel> SimpleEventsViewModel { get; set; }
        public EventListViewModel<MoneyTransferEventViewModel> MoneyTransferEventsViewModel { get; set; }
        public EventListViewModel<RepayDebtEventViewModel> RepayDebtEventsViewModel { get; set; }
        public LimitListViewModel<LimitModel> LimitsViewModel { get; set; }
    }

    public class EventListViewModel<T> : EntityListViewModel<T>
        where T : EventModel
    {
        public IMCCommand ApplyNowCommand { get; set; }
        public IMCCommand CreateTransactionCommand { get; set; }
        public IMCCommand RunCommand { get; set; }
        public IMCCommand PauseCommand { get; set; }
        public IMCCommand CloseCommand { get; set; }
    }

    public class LimitListViewModel<T> : EntityListViewModel<T>
        where T : class
    {
        public IMCCommand RemoveClosedCommand { get; set; }
    }
}
