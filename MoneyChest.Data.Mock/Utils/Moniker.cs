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

        #endregion

        #region Generators

        private Random _rnd = new Random();

        private string RandomWord(string[] words) => words[_rnd.Next(words.Length)];

        #endregion

        #region Sugar 

        public static int Digit => Instance._rnd.Next(10);
        public static string Digits(int length)
        {
            var txt = new StringBuilder();
            for (int i = 0; i < length; i++)
                txt.Append(Digit);
            return txt.ToString();
        }
        
        public static string Category => Instance.RandomWord(Instance._categories);
        public static string UserName => "TestUserName";
        public static string UserPassword => "TestUserPassword";

        public static int Integer(int min, int max) => Instance._rnd.Next(min, max);
        public static int Integer(int max) => Instance._rnd.Next(0, max);

        public static string OneOf(params string[] words) => Instance.RandomWord(words);

        #endregion
    }
}
