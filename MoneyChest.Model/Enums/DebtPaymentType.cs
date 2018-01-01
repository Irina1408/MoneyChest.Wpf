using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Enums
{
    public enum DebtPaymentType
    {
        FixedAmount = 0,
        FixedRate = 1,
        DifferentialPayment = 2,
        AnnuityPayment = 3
    }
}
