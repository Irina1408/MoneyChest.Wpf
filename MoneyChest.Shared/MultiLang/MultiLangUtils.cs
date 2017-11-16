using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.MultiLang
{
    public static class MultiLangUtils
    {
        public static string GetCultureName(Language language)
        {
            switch (language)
            {
                case Language.English:
                    return "en-US";

                case Language.Russian:
                    return "ru-RU";

                default:
                    return "en-US";
            }
        }

        public static Language GetLanguage(string cultureName)
        {
            switch (cultureName)
            {
                case "en-US":
                    return Language.English;

                case "ru-RU":
                    return Language.Russian;

                default:
                    return Language.English;
            }
        }
    }
}
