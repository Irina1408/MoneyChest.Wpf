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
            CategoryIds = new List<int>();
            StorageIds = new List<int>();
        }

        public bool IsFilterApplied { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public TransactionType? TransactionType { get; set; }
        public bool IsCategoryBranchSelection { get; set; }
        
        public List<int> CategoryIds { get; set; }
        public List<int> StorageIds { get; set; }

        public bool IsTransactionTypeFiltered
        {
            get => TransactionType != null;
            set => TransactionType = value ? (TransactionType?)Enums.TransactionType.Expense : null;
        }
    }
}
