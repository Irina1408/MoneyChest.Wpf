using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    [Description("Debt type")]
    public enum DebtType
    {
        [Description("Take borrow")]
        TakeBorrow = 0,

        [Description("Give borrow")]
        GiveBorrow = 1
    }
}
