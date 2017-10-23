using MoneyChest.Calculation.Common;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Summary
{
    public class DebtsSummary : GroupedValueUnitCollection<DebtType>
    {
        public DebtsSummary() : base()
        {
            this.Add(DebtType.GiveBorrow, new ValueUnitCollection());
            this.Add(DebtType.TakeBorrow, new ValueUnitCollection());
        }

        public List<ValueUnit> BorrowedDebts => this[DebtType.TakeBorrow];
        public List<ValueUnit> GivenDebts => this[DebtType.GiveBorrow];
    }
}
