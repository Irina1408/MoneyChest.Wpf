using MoneyChest.Calculation.Summary;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Calculators
{
    public class DebtsSummaryCalculator
    {
        private IDebtService _debtSevice;
        private int _userId;

        public DebtsSummaryCalculator(IDebtService debtSevice, int userId)
        {
            _debtSevice = debtSevice;
            _userId = userId;
        }

        public DebtsSummary CalculateSummary()
        {
            var debtsSummary = new DebtsSummary();

            foreach (var debt in _debtSevice.GetAllForUser(_userId, item => !item.IsRepayed))
                debtsSummary.UpdateBalance(debt.DebtType, debt.CurrencyId, debt.Value - debt.PaidValue);

            return debtsSummary;
        }
    }
}
