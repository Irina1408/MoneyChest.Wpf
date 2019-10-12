using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Services.Converters;
using MoneyChest.Data.Extensions;
using MoneyChest.Services.Utils;
using MoneyChest.Model.Enums;

namespace MoneyChest.Services.Services
{
    public interface ILimitService : IIdManagableUserableListServiceBase<LimitModel>
    {
        List<LimitModel> Get(int userId, DateTime dateFrom, DateTime dateUntil);
        void RemoveClosed(int userId);
    }

    public class LimitService : HistoricizedIdManageableUserableListServiceBase<Limit, LimitModel, LimitConverter>, ILimitService
    {
        private TransactionService _transactionService;
        private ICurrencyExchangeRateService _currencyExchangeRateService;

        public LimitService(ApplicationDbContext context) : base(context)
        {
            _transactionService = new TransactionService(context);
            _currencyExchangeRateService = new CurrencyExchangeRateService(context);
        }

        #region ILimitService implementation

        public List<LimitModel> Get(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            return Scope.Where(x => x.UserId == userId && x.DateFrom <= dateUntil && x.DateUntil >= dateFrom)
                .ToList().ConvertAll(_converter.ToModel);
        }

        public void RemoveClosed(int userId)
        {
            var limitsToRemove = Entities.Where(x => x.UserId == userId && x.DateUntil < DateTime.Today).ToList();
            limitsToRemove.ForEach(entity => Delete(entity));
            SaveChanges();
        }

        #endregion

        #region Overrides

        public override LimitModel Add(LimitModel model)
        {
            // update spend value
            model.SpentValue = CalculateSpendValue(model);

            return base.Add(model);
        }

        public override IEnumerable<LimitModel> Add(IEnumerable<LimitModel> models)
        {
            // update spend value
            foreach(var model in models)
                model.SpentValue = CalculateSpendValue(model);

            return base.Add(models);
        }

        public override LimitModel Update(LimitModel model)
        {
            // update spend value
            model.SpentValue = CalculateSpendValue(model);

            return base.Update(model);
        }

        public override IEnumerable<LimitModel> Update(IEnumerable<LimitModel> models)
        {
            // update spend value
            foreach (var model in models)
                model.SpentValue = CalculateSpendValue(model);

            return base.Update(models);
        }

        public override LimitModel PrepareNew(LimitModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default currency
            var mainCurrency = _context.Currencies.FirstOrDefault(x => x.IsMain && model.UserId == x.UserId);
            model.CurrencyId = mainCurrency?.Id ?? 0;
            model.Currency = mainCurrency?.ToReferenceView();

            return model;
        }

        public override void OnAdded(LimitModel model, Limit entity)
        {
            base.OnAdded(model, entity);

            // update categories list
            UpdateCategoriesList(entity.Id, model.ActualCategoryIds);
            // save changes
            SaveChanges();
        }

        public override void OnUpdated(LimitModel oldModel, LimitModel model)
        {
            base.OnUpdated(oldModel, model);

            // update categories list
            UpdateCategoriesList(model.Id, model.ActualCategoryIds);
            // save changes
            SaveChanges();
        }

        protected override IQueryable<Limit> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Categories);

        #endregion

        #region Private methods

        private void UpdateCategoriesList(int limitId, List<int> categoryIds)
        {
            // load existing categories
            var existing = _context.LimitCategories.Where(x => x.LimitId == limitId).ToList();
            var existingCategoryIds = existing.Select(x => x.CategoryId).ToList();

            // remove extra
            if (existingCategoryIds.Any(x => !categoryIds.Contains(x)))
            {
                _context.LimitCategories.RemoveRange(existing.Where(x => !categoryIds.Contains(x.CategoryId)));
                _context.SaveChanges();
            }

            // add new
            if (categoryIds.Any(x => !existingCategoryIds.Contains(x)))
            {
                _context.LimitCategories.AddRange(categoryIds
                    .Where(x => !existingCategoryIds.Contains(x))
                    .Select(x => new LimitCategory()
                    {
                        LimitId = limitId,
                        CategoryId = x
                    }));
            }
        }

        private decimal CalculateSpendValue(LimitModel model)
        {
            // load actual transactions for the limit
            var transactions = _transactionService.GetActual(model.UserId, model.DateFrom, model.DateUntil, 
                RecordType.Expense, model.IncludeWithoutCategory, model.AllCategories ? null : model.ActualCategoryIds);

            // calculate the same currency amount
            var spentValue = transactions
                .Where(x => x.TransactionCurrencyId == model.CurrencyId)
                ?.Sum(x => x.TransactionAmount) ?? 0;

            // fetch other currency transactions
            var diffCurrTransactions = transactions.Where(x => x.TransactionCurrencyId != model.CurrencyId).ToList();
            if (diffCurrTransactions.Any())
            {
                // load currency exchange rates
                var currencyIds = diffCurrTransactions.Select(x => x.TransactionCurrencyId).Distinct()
                    .Union(new[] { model.CurrencyId }).ToList();
                var currencyExchangeRates = _currencyExchangeRateService.GetList(currencyIds);

                diffCurrTransactions.ForEach(t =>
                {
                    // try to find exchange rate
                    var rate = currencyExchangeRates
                        .FirstOrDefault(x => x.CurrencyFromId == t.TransactionCurrencyId && x.CurrencyToId == model.CurrencyId)
                        ?.Rate;

                    // try to fing an opposite exchange rate
                    if (!rate.HasValue)
                    {
                        rate = currencyExchangeRates
                            .FirstOrDefault(x => x.CurrencyFromId == model.CurrencyId && x.CurrencyToId == t.TransactionCurrencyId)
                            ?.Rate;

                        if (rate.HasValue) rate = 1M / rate;
                    }

                    // check any rate was found
                    if (!rate.HasValue) rate = 1;

                    // update spent value
                    spentValue += t.TransactionAmount * rate.Value;
                });
            }

            return -spentValue;
        }

        #endregion
    }
}
