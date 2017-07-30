using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    [Description("Type")]
    public enum TransactionType
    {
        [Description("Expense (-)")]
        Expense = 0,

        [Description("Income (+)")]
        Income = 1
    }
}
