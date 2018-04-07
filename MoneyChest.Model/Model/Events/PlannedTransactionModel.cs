using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class PlannedTransactionModel<T> : ITransaction
        where T : EventModel
    {
        #region Initialization

        public PlannedTransactionModel()
        { }

        public PlannedTransactionModel(T evnt, DateTime plannedExecutionDate)
        {
            Event = evnt;
            PlannedExecutionDate = plannedExecutionDate;
        }

        #endregion

        #region ITransaction implementation

        public DateTime TransactionDate => PlannedExecutionDate;
        public bool IsPlanned => true;

        public bool IsExpense => Event.IsExpense;
        public bool IsIncome => Event.IsIncome;

        public TransactionType TransactionType => Event.TransactionType;
        public string TransactionValueDetailed => Event.TransactionValueDetailed;
        public string TransactionStorageDetailed => Event.TransactionStorage;
        public int[] TransactionStorageIds => Event.TransactionStorageIds;
        public CategoryReference TransactionCategory => Event.TransactionCategory;

        // General event properties
        public int Id => Event.Id;
        public string Description => Event.Description;
        public string Remark => Event.Remark;

        #endregion

        #region Public properties

        public DateTime PlannedExecutionDate { get; set; }
        public T Event { get; set; }

        #endregion
    }
}
