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

            this.PropertyChanged += (sender, e) =>
            {
                // not use reflection
                if (e.PropertyName == nameof(Date))
                    penalty.Date = Date;
                else if (e.PropertyName == nameof(Description))
                    penalty.Description = Description;
                else if (e.PropertyName == nameof(Value))
                    penalty.Value = Value;
            };
        }

        public ICommand DeleteCommand { get; set; }
    }
}
