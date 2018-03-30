using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SelectableTreeViewStorageGroupModel
    {
        public List<SelectableTreeViewItemModel<StorageModel>> Storages { get; set; } = new List<SelectableTreeViewItemModel<StorageModel>>();
        public bool IsSelected { get; set; }
        public bool IsExpanded { get; set; }
        public StorageGroupReference StorageGroup { get; set; }
    }
}
