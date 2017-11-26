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

        public StorageViewModel(StorageModel model) : this()
        {
            Id = model.Id;
            Name = model.Name;
            Value = model.Value;
            base.IsVisible = model.IsVisible;
            Remark = model.Remark;
            StorageGroupId = model.StorageGroupId;
            CurrencyId = model.CurrencyId;
            UserId = model.UserId;
            Currency = model.Currency;
            StorageGroup = model.StorageGroup;
        }

        public bool IsHidden => !base.IsVisible;
        public bool RemarkExists => !string.IsNullOrEmpty(Remark);

        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
    }
}
