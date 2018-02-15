using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    public class EntityListViewModel<T>
        where T : class
    {
        public ObservableCollection<T> Entities { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand EditCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }
    }
}
