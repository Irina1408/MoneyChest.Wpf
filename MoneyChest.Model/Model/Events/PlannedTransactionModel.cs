using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class PlannedTransactionModel<T> : TransactionBase
        where T : EventModel
    {
        #region Initialization

        public PlannedTransactionModel(T evnt, DateTime plannedExecutionDate)
        {
            Event = evnt;
            PlannedExecutionDate = plannedExecutionDate;

            Id = Event.Id;
            Description = Event.Description;
            Remark = Event.Remark;
        }

        #endregion

        #region Transaction overrides

        public override DateTime TransactionDate => PlannedExecutionDate;
        public override bool IsPlanned => true;

        public override bool IsExpense => Event.IsExpense;
        public override bool IsIncome => Event.IsIncome;

        public override TransactionType TransactionType => Event.TransactionType;
        public override string TransactionValueDetailed => Event.TransactionValueDetailed;
        public override string TransactionStorageDetailed => Event.TransactionStorageDetailed;
        public override int[] TransactionStorageIds => Event.TransactionStorageIds;
        public override CategoryReference TransactionCategory => Event.TransactionCategory;
        public override StorageReference TransactionStorage => Event.TransactionStorage;
        public override int TransactionCurrencyId => Event.TransactionCurrencyId;
        public override decimal TransactionAmount => Event.TransactionAmount;

        #endregion

        #region Public properties

        public DateTime PlannedExecutionDate { get; private set; }
        public T Event { get; private set; }

        #endregion
    }
}
