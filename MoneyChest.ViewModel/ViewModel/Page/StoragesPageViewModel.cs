using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    public class StoragesPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddStorageCommand { get; set; }
        public ICommand EditStorageCommand { get; set; }
        public ICommand DeleteStorageCommand { get; set; }

        public ICommand AddStorageGroupCommand { get; set; }
        public ICommand EditStorageGroupCommand { get; set; }
        public ICommand DeleteStorageGroupCommand { get; set; }
    }
}
