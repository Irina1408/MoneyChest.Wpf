using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Enums;
using System.Reflection;
using System.IO;
using MoneyChest.Utils.FileUtils;
using System.Data.Entity;

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
            AddCategory("Salary", TransactionType.Income);

            // Expense categories
            var transportCategory = AddCategory("Transport", TransactionType.Expense);
            AddCategory("Public transport", TransactionType.Expense, transportCategory);
            AddCategory("Auto", TransactionType.Expense, transportCategory);
            AddCategory("Taxi", TransactionType.Expense, transportCategory);

            var products = AddCategory("Products", TransactionType.Expense);
            AddCategory("Meat", TransactionType.Expense, products);
            AddCategory("Sweets", TransactionType.Expense, products);
            AddCategory("Milk", TransactionType.Expense, products);
            AddCategory("Drink", TransactionType.Expense, products);
            AddCategory("Alcohol", TransactionType.Expense, products);
            AddCategory("Fish", TransactionType.Expense, products);
            AddCategory("Fruits", TransactionType.Expense, products);
            AddCategory("Vegetables", TransactionType.Expense, products);

            var goods = AddCategory("Goods", TransactionType.Expense);
            AddCategory("Appliances", TransactionType.Expense, goods);
            AddCategory("Clothes", TransactionType.Expense, goods);
            AddCategory("Household goods", TransactionType.Expense, goods);

            var entertainment = AddCategory("Entertainment", TransactionType.Expense);
            AddCategory("Cinema", TransactionType.Expense, entertainment);
            AddCategory("Sport", TransactionType.Expense, entertainment);
            AddCategory("Travel", TransactionType.Expense, entertainment);

            // Without type categories
            var other = AddCategory("Other");
            AddCategory("Commission", TransactionType.Expense, other);
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
            _context.GeneralSettings.Add(new GeneralSetting()
            {
                Language = Language.English,
                UserId = _userId
            });

            _context.CalendarSettings.Add(new CalendarSetting() { UserId = _userId });
            _context.ForecastSettings.Add(new ForecastSetting() { UserId = _userId });
            _context.RecordsViewFilters.Add(new RecordsViewFilter() { UserId = _userId });
            _context.ReportSettings.Add(new ReportSetting() { UserId = _userId });
        }

        public void LoadStorages()
        {
            var walletStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Wallet", UserId = _userId });
            var coinBoxStoreGroup = _context.StorageGroups.Add(new StorageGroup() { Name = "Coin box", UserId = _userId });

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
                Name = string.Format("{0} {1} account", currency.Name, storageGroup.Name),
                StorageGroup = storageGroup,
                UserId = _userId,
                Value = 0
            };
            
            _context.Storages.Add(storage);

            return storage;
        }
    }
}
