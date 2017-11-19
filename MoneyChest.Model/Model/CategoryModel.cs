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
            InHistory = false;
        }

        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public TransactionType? TransactionType { get; set; }

        public bool InHistory { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }

        public int? ParentCategoryId { get; set; }
        public int UserId { get; set; }


        // TODO: remove. should be IsActive
        public bool NotInHistory
        {
            get => !InHistory;
            set => InHistory = !value;
        }

        public bool TransactionTypeEnabled
        {
            get => TransactionType.HasValue;
            set => TransactionType = value ? (TransactionType?)Enums.TransactionType.Expense : null;
        }

        public bool TransactionTypeIsExpense
        {
            get => TransactionType.HasValue && TransactionType.Value == Enums.TransactionType.Expense;
            set => TransactionType = value ? Enums.TransactionType.Expense : Enums.TransactionType.Income;
        }

        public bool TransactionTypeIsIncome
        {
            get => TransactionType.HasValue && TransactionType.Value == Enums.TransactionType.Income;
            set => TransactionType = value ? Enums.TransactionType.Income : Enums.TransactionType.Expense;
        }
    }
}
