using MoneyChest.Calculation.Common;
using MoneyChest.Calculation.Summary;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
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

            foreach (var storage in _sevice.GetListForUser(_userId))
                storageSummary.Update(storage.StorageGroup, storage.Currency, storage.Value);

            return storageSummary;
        }

        public StorageSummary CalculateSummary(List<int> storageGroupIds)
        {
            var storageSummary = new StorageSummary();

            foreach (var storage in _sevice.GetList(_userId, storageGroupIds))
                storageSummary.Update(storage.StorageGroup, storage.Currency, storage.Value);

            return storageSummary;
        }

        public SimpleStorageSummary CalculateMainCurrencySummary(ICurrencyService currencyService,
            ICurrencyExchangeRateService currencyExchangeRateService)
        {
            return CalculateSingleCurrencySummary(currencyService.GetMain(_userId), currencyExchangeRateService);
        }

        public SimpleStorageSummary CalculateSingleCurrencySummary(CurrencyModel currency, 
            ICurrencyExchangeRateService currencyExchangeRateService)
        {
            return CalculateSingleCurrencySummary(currency, 
                currencyExchangeRateService.GetList(_userId, currency.Id));
        }

        public SimpleStorageSummary CalculateSingleCurrencySummary(CurrencyModel currency, IEnumerable<CurrencyExchangeRateModel> rates)
        {
            var storageSummary = new SimpleStorageSummary();

            foreach (var storage in _sevice.GetListForUser(_userId))
            {
                storageSummary.Update(storage.StorageGroup, storage.Currency,
                    CalculationHelper.ConvertToCurrency(storage.Value, storage.CurrencyId, currency.Id, rates));
            }

            return storageSummary;
        }

        public SimpleStorageSummary CalculateSingleCurrencySummary(CurrencyModel currency, IEnumerable<CurrencyExchangeRateModel> rates,
            List<int> storageGroupIds)
        {
            var storageSummary = new SimpleStorageSummary();

            foreach (var storage in _sevice.GetList(_userId, storageGroupIds))
            {
                storageSummary.Update(storage.StorageGroup, storage.Currency,
                    CalculationHelper.ConvertToCurrency(storage.Value, storage.CurrencyId, currency.Id, rates));
            }

            return storageSummary;
        }
    }
}
