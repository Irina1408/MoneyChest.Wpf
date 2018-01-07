using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MoneyChest.ViewModel.ViewModel
{
    public class StorageViewModel : StorageModel
    {
        public StorageViewModel() : base()
        { }

        public StorageViewModel(StorageModel storage) : this()
        {
            UpdateData(storage);
        }

        public bool IsHidden => !base.IsVisible;
        public bool RemarkExists => !string.IsNullOrEmpty(Remark);

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand TransferMoneyCommand { get; set; }

        public void UpdateData(StorageModel storage)
        {
            Id = storage.Id;
            Name = storage.Name;
            Value = storage.Value;
            IsVisible = storage.IsVisible;
            Remark = storage.Remark;
            StorageGroupId = storage.StorageGroupId;
            CurrencyId = storage.CurrencyId;
            UserId = storage.UserId;
            Currency = storage.Currency;
            StorageGroup = storage.StorageGroup;
        }
    }
}
