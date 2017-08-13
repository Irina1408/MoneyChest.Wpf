using MoneyChest.Calculation.Common;
using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Summary
{
    public static class SummaryCalculator
    {
        public static StorageSummary CalculateStorageSummary(IEnumerable<Storage> storages)
        {
            var storageSummary = new StorageSummary();

            foreach(var storage in storages)
                storageSummary.UpdateBalance(storage.StorageGroupId, storage.CurrencyId, storage.Value);

            return storageSummary;
        }

        public static DebtsSummary CalculateDebtsSummary(IEnumerable<Debt> debts)
        {
            var debtsSummary = new DebtsSummary();

            foreach (var debt in debts)
                debtsSummary.UpdateBalance(debt.DebtType, debt.CurrencyId, debt.Value - debt.PaidValue);

            return debtsSummary;
        }
    }
}
