using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using MoneyChest.Utils.FileUtils;
using System.Data.Entity;
using MoneyChest.Model.Enums;

namespace MoneyChest.Services.Services.Defaults
{
    internal class DefaultsLoaderEng : IDefaultsLoader
    {
        protected ApplicationDbContext _context;
        protected int _userId;

        public DefaultsLoaderEng(ApplicationDbContext context, int userId)
        {
            _context = context;
            _userId = userId;
        }

        public void LoadCategories()
        {
            // Income categories
            AddCategory("Salary", RecordType.Income);

            // Expense categories
            var transportCategory = AddCategory("Transport", RecordType.Expense);
            AddCategory("Public transport", RecordType.Expense, transportCategory);
            AddCategory("Auto", RecordType.Expense, transportCategory);
            AddCategory("Taxi", RecordType.Expense, transportCategory);

            var products = AddCategory("Products", RecordType.Expense);
            AddCategory("Meat", RecordType.Expense, products);
            AddCategory("Sweets", RecordType.Expense, products);
            AddCategory("Milk", RecordType.Expense, products);
            AddCategory("Drink", RecordType.Expense, products);
            AddCategory("Alcohol", RecordType.Expense, products);
            AddCategory("Fish", RecordType.Expense, products);
            AddCategory("Fruits", RecordType.Expense, products);
            AddCategory("Vegetables", RecordType.Expense, products);

            var goods = AddCategory("Goods", RecordType.Expense);
            AddCategory("Appliances", RecordType.Expense, goods);
            AddCategory("Clothes", RecordType.Expense, goods);
            AddCategory("Household goods", RecordType.Expense, goods);

            var entertainment = AddCategory("Entertainment", RecordType.Expense);
            AddCategory("Cinema", RecordType.Expense, entertainment);
            AddCategory("Sport", RecordType.Expense, entertainment);
            AddCategory("Travel", RecordType.Expense, entertainment);

            // Without type categories
            var other = AddCategory("Other");
            AddCategory("Debts", null, other);
            AddCategory("Gifts", null, other);
        }

        public void LoadCurrencies()
        {
            string filePath = Directory.GetCurrentDirectory() + @"\currencies.csv";
            Assembly asm = Assembly.GetExecutingAssembly();
            string file = string.Format("{0}.CurrenciesEng.csv", asm.GetName().Name);
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
                Language = Language.English,
                UserId = _userId
            });

            _context.CalendarSettings.Add(new CalendarSettings()
            {
                UserId = _userId,
                ShowLimits = true,
                DataFilter = _context.DataFilters.Add(new DataFilter()
                {
                    IncludeWithoutCategory = true
                }),
                PeriodFilter = _context.PeriodFilters.Add(new PeriodFilter()
                {
                    PeriodType = PeriodType.Month
                })
            });
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
            _context.ReportSettings.Add(new ReportSetting()
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
        }

        public void LoadStorages()
        {
            var walletStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Wallet", UserId = _userId });
            var coinBoxStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Coin box", UserId = _userId });

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
