using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    public class StorageGroupViewModel : StorageGroupModel
    {
        public StorageGroupViewModel() : base()
        {

        }

        public StorageGroupViewModel(StorageGroupModel model) : this()
        {
            Id = model.Id;
            Name = model.Name;
            UserId = model.UserId;

            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(Name))
                    IsChanged = true;
            };

            IsChanged = false;
        }

        public bool IsChanged { get; set; }
        public ICommand AddStorageCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
    }
}
