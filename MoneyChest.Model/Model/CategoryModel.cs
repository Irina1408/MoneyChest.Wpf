using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class CategoryModel : IHasId, IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CategoryModel()
        {
            IsActive = true;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public RecordType? RecordType { get; set; }

        public bool IsActive { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }

        public int? ParentCategoryId { get; set; }
        public int UserId { get; set; }

        
        public bool RecordTypeEnabled
        {
            get => RecordType.HasValue;
            set => RecordType = value ? (RecordType?)Enums.RecordType.Expense : null;
        }

        public bool RecordTypeIsExpense
        {
            get => RecordType.HasValue && RecordType.Value == Enums.RecordType.Expense;
            set => RecordType = value ? Enums.RecordType.Expense : Enums.RecordType.Income;
        }

        public bool RecordTypeIsIncome
        {
            get => RecordType.HasValue && RecordType.Value == Enums.RecordType.Income;
            set => RecordType = value ? Enums.RecordType.Income : Enums.RecordType.Expense;
        }
    }
}
