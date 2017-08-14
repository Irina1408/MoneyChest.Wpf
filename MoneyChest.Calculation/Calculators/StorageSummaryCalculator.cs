using MoneyChest.Calculation.Summary;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Calculators
{
    public class StorageSummaryCalculator
    {
        private IStorageService _sevice;
        private int _userId;

        public StorageSummaryCalculator(IStorageService sevice, int userId)
        {
            _sevice = sevice;
            _userId = userId;
        }

        public StorageSummary CalculateSummary()
        {
            var storageSummary = new StorageSummary();

            foreach (var storage in _sevice.GetAllForUser(_userId))
                storageSummary.UpdateBalance(storage.StorageGroupId, storage.CurrencyId, storage.Value);

            return storageSummary;
        }
    }
}
