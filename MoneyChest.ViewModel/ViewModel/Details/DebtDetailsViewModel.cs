using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    public class DebtDetailsViewModel : DetailsViewCommandContainer
    {
        public ICommand AddPenaltyCommand { get; set; }
    }
}
