using MoneyChest.Model.Enums;
using MoneyChest.Utils.SerializationUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.Settings
{
    public class AppSettings
    {
        #region Singltone

        private static AppSettings currentSettings;

        public static AppSettings Instance => 
            currentSettings ?? (currentSettings = ObjectSerializer.Load<AppSettings>() ?? new AppSettings());

        #endregion

        #region Public properties

        public string LastLogin { get; set; }
        public Language? LastLanguage { get; set; }
        public bool IsMenuHeadersShown { get; set; } = true;

        #endregion

        #region Public methods

        public void Save()
        {
            ObjectSerializer.Save(this);
        }

        #endregion
    }
}
