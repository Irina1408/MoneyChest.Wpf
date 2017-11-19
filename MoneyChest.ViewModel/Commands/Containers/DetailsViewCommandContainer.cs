using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.Commands
{
    public class DetailsViewCommandContainer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IMCCommand SaveCommand { get; set; }
        public IMCCommand CancelCommand { get; set; }
    }
}
