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
        }

        public EventListViewModel<SimpleEventViewModel> SimpleEventsViewModel { get; set; }
        public EventListViewModel<MoneyTransferEventViewModel> MoneyTransferEventsViewModel { get; set; }
        public EventListViewModel<RepayDebtEventViewModel> RepayDebtEventsViewModel { get; set; }
    }

    public class EventListViewModel<T>
        where T : class
    {
        public ObservableCollection<T> Events { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
    }
}
