using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Shared.MultiLang;
using MoneyChest.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ChequeViewModel
    {
        public ChequeViewModel()
        {
            Date = DateTime.Now;
            Entities = new ObservableCollection<RecordModel>();
        }

        public ObservableCollection<RecordModel> Entities { get; set; }

        public IMCCommand AddCommand { get; set; }
        public IMCCommand DuplicateCommand { get; set; }
        public IMCCommand DeleteCommand { get; set; }

        public IMCCommand SaveCommand { get; set; }
        public IMCCommand CancelCommand { get; set; }

        public ObservableCollection<MultiLangEnumDescription> RecordTypeList { get; set; }

        public decimal TotalAmount { get; set; }
        public string TotalAmountDetailed => Currency?.FormatValue(TotalAmount) ?? TotalAmount.ToString();

        // default field values
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Remark { get; set; }
        public int CurrencyId { get; set; }
        public CurrencyReference Currency { get; set; }
        public int StorageId { get; set; }
        public StorageReference Storage { get; set; }
        public decimal CurrencyExchangeRate { get; set; } = 1;

        public void RefreshTotalAmount() => TotalAmount = Entities.Sum(x => x.RecordType == RecordType.Expense ? -x.Value : x.Value);
    }
}
