using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Constants;
using MoneyChest.Model.Enums;

namespace MoneyChest.Model.Model
{
    public class TransactionTemplateBase : ITransactionTemplate, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region ITransactionTemplate implementation
        
        [StringLength(MaxSize.NameLength)]
        public string Name { get; set; }
        public virtual bool IsExpense => TransactionType == TransactionType.Expense;
        public virtual bool IsIncome => TransactionType == TransactionType.Income;
        public virtual TransactionType TransactionType { get; set; }

        #endregion
    }
}
