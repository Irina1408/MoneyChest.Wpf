using MoneyChest.Calculation.Common;
using MoneyChest.Calculation.Summary.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Summary
{
    public class DebtsSummary : BaseSummary<DebtType>
    {
        public List<ValueUnit> BorrowedDebts => Items.Where(_ => _.Special == DebtType.TakeBorrow).Select(_ => _ as ValueUnit).ToList();
        public List<ValueUnit> GivenDebts => Items.Where(_ => _.Special == DebtType.GiveBorrow).Select(_ => _ as ValueUnit).ToList();
    }
}
