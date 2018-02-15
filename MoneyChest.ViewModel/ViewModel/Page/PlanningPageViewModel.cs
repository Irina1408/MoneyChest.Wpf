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
            SimpleEventsViewModel = new EntityListViewModel<SimpleEventViewModel>();
            MoneyTransferEventsViewModel = new EntityListViewModel<MoneyTransferEventViewModel>();
            RepayDebtEventsViewModel = new EntityListViewModel<RepayDebtEventViewModel>();
            LimitsViewModel = new EntityListViewModel<LimitModel>();
        }

        public EntityListViewModel<SimpleEventViewModel> SimpleEventsViewModel { get; set; }
        public EntityListViewModel<MoneyTransferEventViewModel> MoneyTransferEventsViewModel { get; set; }
        public EntityListViewModel<RepayDebtEventViewModel> RepayDebtEventsViewModel { get; set; }
        public EntityListViewModel<LimitModel> LimitsViewModel { get; set; }
    }
}
