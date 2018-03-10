using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public interface ITransaction : IHasId
    {
        bool IsPlanned { get; }
        bool IsExpense { get; }
        bool IsIncome { get; }
        TransactionType TransactionType { get; }
        DateTime TransactionDate { get; }
        string TransactionValueDetailed { get; }
        string TransactionStorage { get; }
        CategoryReference TransactionCategory { get; }

        // General properties
        string Description { get; set; }
        string Remark { get; set; }
    }
}
