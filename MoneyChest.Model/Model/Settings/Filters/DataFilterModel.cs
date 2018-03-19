using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DataFilterModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DataFilterModel()
        {
            IsSingleCategorySelection = false;
            IncludeWithoutCategory = true;
            CategoryIds = new List<int>();
            StorageIds = new List<int>();
        }

        public bool IsFilterApplied { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public TransactionType? TransactionType { get; set; }
        
        public bool IsSingleCategorySelection { get; set; }
        public bool IncludeWithoutCategory { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<int> StorageIds { get; set; }
    }
}
