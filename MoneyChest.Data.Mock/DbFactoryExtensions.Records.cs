using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Mock.Utils;

namespace MoneyChest.Data.Mock
{
    public static partial class DbFactoryExtensions
    {
        public static Record CreateRecord(this DbFactory f, int userId, Action<Record> overrides = null)
        {
            var storage = f.CreateStorage(userId);
            var category = f.Create<Category>(_ => _.UserId = userId);

            return f.Create<Record>(item =>
            {
                item.CategoryId = category.Id;
                item.StorageId = storage.Id;
                item.CurrencyId = storage.CurrencyId;
                item.UserId = userId;
                overrides?.Invoke(item);
            });
        }

        public static List<Record> CreateRecords(this DbFactory f, int userId, DateTime from, DateTime until, Action<Record> overrides = null)
        {
            return CreateRecords(f, userId, from, until, Moniker.LimitedDigit(2, 5), overrides);
        }

        public static List<Record> CreateRecords(this DbFactory f, int userId, DateTime from, DateTime until, int count, Action<Record> overrides = null)
        {
            var storage = f.CreateStorage(userId);
            var category = f.Create<Category>(_ => _.UserId = userId);
            var result = new List<Record>();

            // create boundary values
            if(count >= 2)
            {
                result.Add(f.Create<Record>(item =>
                {
                    item.CategoryId = category.Id;
                    item.StorageId = storage.Id;
                    item.CurrencyId = storage.CurrencyId;
                    item.Date = from;
                    item.UserId = userId;
                    overrides?.Invoke(item);
                }));
                result.Add(f.Create<Record>(item =>
                {
                    item.CategoryId = category.Id;
                    item.StorageId = storage.Id;
                    item.CurrencyId = storage.CurrencyId;
                    item.Date = until;
                    item.UserId = userId;
                    overrides?.Invoke(item);
                }));
                count -= 2;
            }

            while (count-- > 0)
            {
                result.Add(f.Create<Record>(item =>
                {
                    item.CategoryId = category.Id;
                    item.StorageId = storage.Id;
                    item.CurrencyId = storage.CurrencyId;
                    item.Date = Moniker.DateTimeBetween(from, until);
                    item.UserId = userId;
                    overrides?.Invoke(item);
                }));
            }

            return result;
        }
    }
}
