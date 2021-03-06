﻿using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public interface ITransactionTemplate
    {
        string Name { get; }
        bool IsExpense { get; }
        bool IsIncome { get; }
        TransactionType TransactionType { get; }
    }
}
