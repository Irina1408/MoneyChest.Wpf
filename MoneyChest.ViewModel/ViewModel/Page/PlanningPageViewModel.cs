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
            LimitsViewModel = new EntityListViewModel<LimitModel>();
        }

        public EventListViewModel<SimpleEventViewModel> SimpleEventsViewModel { get; set; }
        public EventListViewModel<MoneyTransferEventViewModel> MoneyTransferEventsViewModel { get; set; }
        public EventListViewModel<RepayDebtEventViewModel> RepayDebtEventsViewModel { get; set; }
        public EntityListViewModel<LimitModel> LimitsViewModel { get; set; }
    }

    public class EventListViewModel<T> : EntityListViewModel<T>
        where T : EventModel
    {
        public IMCCommand ApplyNowCommand { get; set; }
        public IMCCommand CreateTransactionCommand { get; set; }
    }
}
