using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Enums;
using System.Reflection;
using System.IO;
using MoneyChest.Utils.FileUtils;

namespace MoneyChest.Services.Services.Defaults
{
    internal class DefaultsLoaderRus : IDefaultsLoader
    {
        protected ApplicationDbContext _context;
        protected int _userId;

        public DefaultsLoaderRus(ApplicationDbContext context, int userId)
        {
            _context = context;
            _userId = userId;
        }

        public void LoadCategories()
        {
            // Income categories
            AddCategory("Зарплата", TransactionType.Income);

            // Expense categories
            var transportCategory = AddCategory("Транспорт", TransactionType.Expense);
            AddCategory("Общественный транспорт", TransactionType.Expense, transportCategory);
            AddCategory("Машина", TransactionType.Expense, transportCategory);
            AddCategory("Такси", TransactionType.Expense, transportCategory);

            var products = AddCategory("Продукты", TransactionType.Expense);
            AddCategory("Мясо", TransactionType.Expense, products);
            AddCategory("Сладости", TransactionType.Expense, products);
            AddCategory("Молоко", TransactionType.Expense, products);
            AddCategory("Напитки", TransactionType.Expense, products);
            AddCategory("Алкоголь", TransactionType.Expense, products);
            AddCategory("Рыба", TransactionType.Expense, products);
            AddCategory("Фрукты", TransactionType.Expense, products);
            AddCategory("Овощи", TransactionType.Expense, products);

            var goods = AddCategory("Товары", TransactionType.Expense);
            AddCategory("Техника", TransactionType.Expense, goods);
            AddCategory("Одежда", TransactionType.Expense, goods);
            AddCategory("Бытовые товары", TransactionType.Expense, goods);

            var entertainment = AddCategory("Развлечения", TransactionType.Expense);
            AddCategory("Кино", TransactionType.Expense, entertainment);
            AddCategory("Спорт", TransactionType.Expense, entertainment);
            AddCategory("Путешествия", TransactionType.Expense, entertainment);

            // Without type categories
            var other = AddCategory("Разное");
            AddCategory("Комиссия", TransactionType.Expense, other);
            AddCategory("Долги", null, other);
            AddCategory("Подарки", null, other);
        }

        public void LoadCurrencies()
        {
            string filePath = Directory.GetCurrentDirectory() + @"\currencies.csv";
            Assembly asm = Assembly.GetExecutingAssembly();
            string file = string.Format("{0}.CurrenciesRus.csv", asm.GetName().Name);
            Stream fileStream = asm.GetManifestResourceStream(file);
            FileHelper.SaveStreamToFile(filePath, fileStream);

            var currencies = new List<Currency>();

            using (var csvReader = new CsvTableReader(filePath))
            {
                while (csvReader.ReadLine())
                {
                    var name = csvReader.CurrentRowValues["Name"];

                    // only distinct currencies
                    if (_context.Currencies
                        .FirstOrDefault(item => item.Name == name && item.UserId == _userId) == null)
                    {
                        // create new currency
                        var currency = new Currency
                        {
                            Name = name,
                            Code = csvReader.CurrentRowValues["Code"],
                            Symbol = csvReader.CurrentRowValues["Symbol"],
                            IsUsed = csvReader.CurrentRowValues["Used"] == "+",
                            IsMain = csvReader.CurrentRowValues["Main"] == "+",
                            UserId = _userId
                        };

                        // add to currency list
                        _context.Currencies.Add(currency);
                    }
                }
            }

            FileHelper.RemoveFile(filePath);
        }

        public void LoadSettings()
        {
            var debtsCategory = _context.Categories.FirstOrDefault(item => item.UserId == _userId && item.Name == "Долги");
            var commissionCategory = _context.Categories.FirstOrDefault(item => item.UserId == _userId && item.Name == "Комиссия");

            if (debtsCategory == null)
                debtsCategory = AddCategory("Долги");
            if (commissionCategory == null)
                commissionCategory = AddCategory("Комиссия");

            _context.GeneralSettings.Add(new GeneralSetting()
            {
                Language = Language.Russian,
                UserId = _userId,
                ComissionCategory = commissionCategory,
                DebtCategory = debtsCategory
            });

            _context.CalendarSettings.Add(new CalendarSetting() { UserId = _userId });
            _context.ForecastSettings.Add(new ForecastSetting() { UserId = _userId });
            _context.RecordsViewFilters.Add(new RecordsViewFilter() { UserId = _userId });
            _context.ReportSettings.Add(new ReportSetting() { UserId = _userId });
        }

        public void LoadStorages()
        {
            var walletStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Кошелек", UserId = _userId });
            var coinBoxStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Копилка", UserId = _userId });

            foreach (var currency in _context.Currencies.Where(item => item.UserId == _userId && item.IsUsed).ToList())
            {
                AddStorage(walletStoreGroup, currency);
                AddStorage(coinBoxStoreGroup, currency);
            }
        }

        private Category AddCategory(string name, TransactionType? transactionType = null, Category parent = null)
        {
            var category = new Category()
            {
                Name = name,
                TransactionType = transactionType,
                ParentCategory = parent,
                UserId = _userId
            };

            _context.Categories.Add(category);

            return category;
        }

        private Storage AddStorage(StorageGroup storageGroup, Currency currency)
        {
            var storage = new Storage()
            {
                Currency = currency,
                CurrencyId = currency.Id,
                Name = string.Format("{0} {1}", currency.Name, storageGroup.Name),
                StorageGroup = storageGroup,
                UserId = _userId,
                Value = 0
            };

            _context.Storages.Add(storage);

            return storage;
        }
    }
}
