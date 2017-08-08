using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Defaults
{
    internal interface IDefaultsLoader
    {
        void LoadCategories();
        void LoadStorages();
        void LoadCurrencies();
        void LoadSettings();
    }
}
