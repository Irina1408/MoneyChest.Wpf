using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;

namespace MoneyChest.Model.Model
{
    public abstract class TransactionBase : ITransaction
    {
        public abstract bool IsPlanned { get; }
        public virtual bool IsExpense => TransactionType == TransactionType.Expense;
        public virtual bool IsIncome => TransactionType == TransactionType.Income;

        public abstract TransactionType TransactionType { get; }
        public abstract DateTime TransactionDate { get; }
        public abstract CategoryReference TransactionCategory { get; }
        public abstract int TransactionCurrencyId { get; }
        public abstract decimal TransactionAmount { get; }
        public abstract StorageReference TransactionStorage { get; }

        public abstract string TransactionValueDetailed { get; }
        public abstract string TransactionStorageDetailed { get; }

        public virtual int Id { get; set; }
        public virtual string Description { get; set; }
        public virtual string Remark { get; set; }

        public abstract int[] TransactionStorageIds { get; }
    }
}
