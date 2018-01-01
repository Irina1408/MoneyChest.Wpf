using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    public class DebtPenaltyViewModel : DebtPenaltyModel
    {
        public DebtPenaltyViewModel() : base()
        { }

        public DebtPenaltyViewModel(DebtPenaltyModel penalty) : this()
        {
            Id = penalty.Id;
            Date = penalty.Date;
            Description = penalty.Description;
            Value = penalty.Value;
            DebtId = penalty.DebtId;
        }

        public ICommand DeleteCommand { get; set; }
    }
}
