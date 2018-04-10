using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    /// <summary>
    /// Markers for color and opasity
    /// </summary>
    public interface ITransactionMarkers
    {
        bool IsPlanned { get; }
        bool IsExpense { get; }
        bool IsIncome { get; }
    }

    /// <summary>
    /// Properties for showing
    /// </summary>
    public interface ITransactionViewDetails
    {
        string TransactionValueDetailed { get; }
        string TransactionStorageDetailed { get; }
    }

    /// <summary>
    /// Properties for filtering
    /// </summary>
    public interface ITransactionValueTransfering
    {
        //CurrencyReference TransactionCurrency { get; }
        //decimal TransactionAmount { get; }
        int[] TransactionStorageIds { get; }
    }

    /// <summary>
    /// General entities properties
    /// </summary>
    public interface ITransactionEntity
    {
        int Id { get; }
        string Description { get; }
        string Remark { get; }
    }

    public interface ITransaction : ITransactionMarkers, ITransactionViewDetails, ITransactionValueTransfering, ITransactionEntity
    {
        // General transaction info
        TransactionType TransactionType { get; }
        DateTime TransactionDate { get; }
        CategoryReference TransactionCategory { get; }
        StorageReference TransactionStorage { get; }
        int TransactionCurrencyId { get; }
        decimal TransactionAmount { get; }
    }
}
