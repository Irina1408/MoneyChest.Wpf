using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Mock.Utils
{
    class Moniker
    {
        private static Moniker Instance = new Moniker();

        #region dictionaries 

        private string[] _categories = new string[]
        {
            "Products", "Goods", "Appliances", "Clothes", "Entertainment", "Cinema", "Sport", "Travel", "Auto", "Gifts", "Debts", "Other",
            "Meat", "Sweets", "Milk", "Drink", "Alcohol", "Fish", "Fruits", "Vegetables", "Transport"
        };

        private string[] _currencies = new string[]
        {
            "Hryvnia", "US Dollar", "Euro", "Russian Ruble", "Belarussian Ruble"
        };

        private string[] _storageGroups = new string[]
        {
            "Wallet", "Coin box"
        };

        #endregion

        #region Generators

        private Random _rnd = new Random();

        private string RandomWord(string[] words) => words[_rnd.Next(words.Length)];

        #endregion

        #region Sugar 

        public static int Digit => Instance._rnd.Next(1000);
        public static int LimitedDigit(int maxValue) => Instance._rnd.Next(maxValue);
        public static int LimitedDigit(int minValue, int maxValue) => Instance._rnd.Next(minValue, maxValue);
        public static string Digits(int length)
        {
            var txt = new StringBuilder();
            for (int i = 0; i < length; i++)
                txt.Append(LimitedDigit(9));
            return txt.ToString();
        }
        public static DateTime DateTimeBetween(DateTime from, DateTime until) =>
            from.AddHours(Instance._rnd.Next((int)(until - from).TotalHours));
            
        public static string Category => Instance.RandomWord(Instance._categories);
        public static string Currency => Instance.RandomWord(Instance._currencies);
        public static string StorageGroup => Instance.RandomWord(Instance._storageGroups);
        public static string CurrencyCode => "COD";
        public static string CurrencySymbol => "₴";
        public static string UserName => "TestUserName";
        public static string UserPassword => "TestUserPassword";

        public static int Integer(int min, int max) => Instance._rnd.Next(min, max);
        public static int Integer(int max) => Instance._rnd.Next(0, max);

        public static string OneOf(params string[] words) => Instance.RandomWord(words);

        #endregion
    }
}
