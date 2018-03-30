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
using MoneyChest.Model.Enums;

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
            AddCategory("Зарплата", RecordType.Income);

            // Expense categories
            var transportCategory = AddCategory("Транспорт", RecordType.Expense);
            AddCategory("Общественный транспорт", RecordType.Expense, transportCategory);
            AddCategory("Машина", RecordType.Expense, transportCategory);
            AddCategory("Такси", RecordType.Expense, transportCategory);

            var products = AddCategory("Продукты", RecordType.Expense);
            AddCategory("Мясо", RecordType.Expense, products);
            AddCategory("Сладости", RecordType.Expense, products);
            AddCategory("Молоко", RecordType.Expense, products);
            AddCategory("Напитки", RecordType.Expense, products);
            AddCategory("Алкоголь", RecordType.Expense, products);
            AddCategory("Рыба", RecordType.Expense, products);
            AddCategory("Фрукты", RecordType.Expense, products);
            AddCategory("Овощи", RecordType.Expense, products);

            var goods = AddCategory("Товары", RecordType.Expense);
            AddCategory("Техника", RecordType.Expense, goods);
            AddCategory("Одежда", RecordType.Expense, goods);
            AddCategory("Бытовые товары", RecordType.Expense, goods);

            var entertainment = AddCategory("Развлечения", RecordType.Expense);
            AddCategory("Кино", RecordType.Expense, entertainment);
            AddCategory("Спорт", RecordType.Expense, entertainment);
            AddCategory("Путешествия", RecordType.Expense, entertainment);

            // Without type categories
            var other = AddCategory("Разное");
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
                        var code = csvReader.CurrentRowValues["Code"];
                        var symbol = csvReader.CurrentRowValues["Symbol"];

                        if (string.IsNullOrEmpty(symbol))
                            symbol = code;

                        // create new currency
                        var currency = new Currency
                        {
                            Name = name,
                            Code = code,
                            Symbol = symbol,
                            IsActive = csvReader.CurrentRowValues["Used"] == "+",
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
            _context.GeneralSettings.Add(new GeneralSetting()
            {
                Language = Language.Russian,
                UserId = _userId
            });

            _context.CalendarSettings.Add(new CalendarSettings() { UserId = _userId });
            _context.ForecastSettings.Add(new ForecastSetting() { UserId = _userId });
            _context.TransactionsSettings.Add(new TransactionsSettings()
            {
                UserId = _userId,
                DataFilter = _context.DataFilters.Add(new DataFilter()
                {
                    IncludeWithoutCategory = true
                }),
                PeriodFilter = _context.PeriodFilters.Add(new PeriodFilter()
                {
                    PeriodType = PeriodType.Month
                })
            });
            _context.ReportSettings.Add(new ReportSetting() { UserId = _userId });
        }

        public void LoadStorages()
        {
            var walletStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Кошелек", UserId = _userId });
            var coinBoxStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Копилка", UserId = _userId });

            foreach (var currency in _context.Currencies.Where(item => item.UserId == _userId && item.IsActive).ToList())
            {
                AddStorage(walletStoreGroup, currency);
                AddStorage(coinBoxStoreGroup, currency);
            }
        }

        private Category AddCategory(string name, RecordType? recordType = null, Category parent = null)
        {
            var category = new Category()
            {
                Name = name,
                RecordType = recordType,
                ParentCategory = parent,
                IsActive = true,
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
                IsVisible = true,
                Value = 0
            };

            _context.Storages.Add(storage);

            return storage;
        }
    }
}
