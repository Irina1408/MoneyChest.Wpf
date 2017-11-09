using MoneyChest.Data.Context;
using MoneyChest.Model.Enums;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Defaults
{
    public class DefaultsLoadingService : ServiceBase
    {
        public DefaultsLoadingService(ApplicationDbContext context) : base(context)
        {
        }

        public void LoadDefaults(int userId, Language language)
        {
            var loader = (language == Language.Russian) 
                ? (IDefaultsLoader)new DefaultsLoaderRus(_context, userId) 
                : (IDefaultsLoader)new DefaultsLoaderEng(_context, userId);

            loader.LoadCategories();
            SaveChanges();
            loader.LoadCurrencies();
            SaveChanges();
            loader.LoadStorages();
            SaveChanges();
            loader.LoadSettings();
            SaveChanges();
        }

        public void LoadDefaultSettingss(int userId, Language language)
        {
            var loader = (language == Language.Russian)
                ? (IDefaultsLoader)new DefaultsLoaderRus(_context, userId)
                : (IDefaultsLoader)new DefaultsLoaderEng(_context, userId);
            
            loader.LoadSettings();
            SaveChanges();
        }
    }
}
