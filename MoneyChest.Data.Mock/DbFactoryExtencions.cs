using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Mock
{
    public static class DbFactoryExtencions
    {
        public static Storage CreateStorage(this DbFactory f, int userId, Action<Storage> overrides = null)
        {
            var currency = f.Create<Currency>(item => item.UserId = userId);
            var storageGroup = f.Create<StorageGroup>(item => item.UserId = userId);
            return f.Create<Storage>(item =>
            {
                item.UserId = userId;
                item.CurrencyId = currency.Id;
                item.StorageGroupId = storageGroup.Id;
                overrides?.Invoke(item);
            });
        }

        public static Debt CreateDebt(this DbFactory f, int userId, Action<Debt> overrides = null)
        {
            var storage = CreateStorage(f, userId);
            return f.Create<Debt>(item =>
            {
                item.UserId = userId;
                item.CurrencyId = storage.CurrencyId;
                item.StorageId = storage.Id;
                overrides?.Invoke(item);
            });
        }
        
        public static CurrencyExchangeRate CreateCurrencyExchangeRate(this DbFactory f, int userId, Action<CurrencyExchangeRate> overrides = null)
        {
            var currency1 = f.Create<Currency>(item => item.UserId = userId);
            var currency2 = f.Create<Currency>(item => item.UserId = userId);

            return f.Create<CurrencyExchangeRate>(item =>
            {
                item.CurrencyFromId = currency1.Id;
                item.CurrencyToId = currency2.Id;
                overrides?.Invoke(item);
            });
        }

        public static MoneyTransferEvent CreateMoneyTransferEvent(this DbFactory f, int userId, Action<MoneyTransferEvent> overrides = null)
        {
            var storage1 = CreateStorage(f, userId);
            var storage2 = CreateStorage(f, userId);

            return f.Create<MoneyTransferEvent>(item =>
            {
                item.StorageFromId = storage1.Id;
                item.StorageToId = storage2.Id;
                item.UserId = userId;
                overrides?.Invoke(item);
            });
        }

        public static SimpleEvent CreateSimpleEvent(this DbFactory f, int userId, Action<SimpleEvent> overrides = null)
        {
            var storage = CreateStorage(f, userId);
            var category = f.Create<Category>(item => item.UserId = userId);

            return f.Create<SimpleEvent>(item =>
            {
                item.StorageId = storage.Id;
                item.CurrencyId = storage.CurrencyId;
                item.UserId = userId;
                item.CategoryId = category.Id;
                overrides?.Invoke(item);
            });
        }
    }
}
