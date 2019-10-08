using MoneyChest.Data.Context;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Converters;
using MoneyChest.Services.Services.Base;
using MoneyChest.Services.Services.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public interface ITransactionDependenceService
    {
        void AddValueToStorage(int storageId, decimal value);
        void AddValueToDebt(int debtId, decimal value);
        void UpdateLimits(DateTime date, int? categoryId, int currencyId, decimal spentValue, 
            int? currencyForRateId, decimal spentValueByRate);
    }

    public class TransactionDependenceService : ServiceBase, ITransactionDependenceService
    {
        internal HistoryService _historyService;

        public TransactionDependenceService(ApplicationDbContext context) : base(context)
        {
            _historyService = new HistoryService(context);
        }

        public void AddValueToStorage(int storageId, decimal value)
        {
            // find storage in database
            var storage = _context.Storages.FirstOrDefault(_ => _.Id == storageId);
            // add provided value (it's expected that the value in the correct currency)
            storage.Value += value;
            // update history
            _historyService.WriteHistory(storage, ActionType.Update, storage.UserId);
        }

        public void AddValueToDebt(int debtId, decimal value)
        {
            // load debt and update paid value
            var debt = _context.Debts.Include(_ => _.DebtPenalties).FirstOrDefault(_ => _.Id == debtId);
            debt.PaidValue += value;
            // update flag 'IsRepaid' including all commissions, penalties etc.
            var debtConverter = new DebtConverter();
            debt.IsRepaid = debtConverter.ToModel(debt).RemainsToPay <= 0;
            // update history
            _historyService.WriteHistory(debt, ActionType.Update, debt.UserId);
        }

        public void UpdateLimits(DateTime date, int? categoryId, int currencyId, decimal spentValue,
            int? currencyForRateId, decimal spentValueByRate)
        {
            // check there is any value
            if (spentValue == 0 || spentValueByRate == 0) return;

            // create limits filter by categories
            Expression<Func<Limit, bool>> limitsCategoryFilter = x => true;
            if (categoryId.HasValue) limitsCategoryFilter = x => x.AllCategories || x.Categories.Any(_ => _.CategoryId == categoryId);
            else limitsCategoryFilter = x => x.IncludeWithoutCategory;

            // load limits
            var limits = _context.Limits
                .Include(_ => _.Categories)
                .Where(x => x.DateFrom <= date && date <= x.DateUntil)
                .Where(limitsCategoryFilter)
                .ToList();

            // load currency exchange rates for limits with different currencies
            List<CurrencyExchangeRate> rates = null;
            if (limits.Any(x => x.CurrencyId != currencyId))
                rates = _context.CurrencyExchangeRates.ToList();

            // update limits in loop
            foreach (var limit in limits)
            {
                // update limits with the same currencies
                if (limit.CurrencyId == currencyId)
                    limit.SpentValue += spentValue;
                else if (currencyForRateId.HasValue && currencyForRateId != currencyId && limit.CurrencyId == currencyForRateId)
                    limit.SpentValue += spentValueByRate;
                else
                {
                    // update limits with different currencies correspond to provided rate
                    // try to find exchange rate
                    var rate = rates.FirstOrDefault(x => x.CurrencyFromId == currencyId && x.CurrencyToId == limit.CurrencyId)?.Rate;

                    // try to fing an opposite exchange rate
                    if (!rate.HasValue)
                    {
                        // find existing rate
                        rate = rates.FirstOrDefault(x => x.CurrencyFromId == limit.CurrencyId && x.CurrencyToId == currencyId)?.Rate;
                        // adapt rate
                        if (rate.HasValue) rate = 1M / rate;
                    }

                    // check any rate was found
                    if (!rate.HasValue) rate = 1;

                    // update limit value
                    limit.SpentValue += spentValue * rate.Value;
                }
            }
        }
    }
}
