using MoneyChest.Utils.SerializationUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.Settings
{
    public class UserSettings
    {
        #region Singltone

        private static UserSettings currentSettings;

        public static UserSettings Instance
        {
            get
            {
                return currentSettings ?? (currentSettings = ObjectSerializer.Load<UserSettings>() ?? new UserSettings());
            }
        }

        #endregion

        #region Public properties

        public string LastLogin { get; set; }

        #endregion

        #region Public methods

        public void Save()
        {
            ObjectSerializer.Save(this);
        }

        #endregion
    }
}
