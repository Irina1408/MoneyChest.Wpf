using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Shared.MultiLang
{
    public class MultiLangResourceManager : INotifyPropertyChanged
    {
        private ResourceManager _resourseManager;

        public MultiLangResourceManager()
        {
            _resourseManager = Properties.Resources.ResourceManager;
            _resourseManager.IgnoreCase = true;
        }

        [IndexerName("Item")]
        public string this[string resourceName]
        {
            get
            {
                try
                {
                    return _resourseManager.GetString(resourceName);
                }
                catch (MissingManifestResourceException)
                {
                    return null;
                }
            }
        }

        public void SetLanguage(Language language)
        {
            SetCulture(MultiLangUtils.GetCultureName(language));
        }

        public void SetCulture(string name)
        {
            if (CultureInfo.CurrentUICulture.Name == name) return;

            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo(name);
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));
            CultureChanged?.Invoke(this, new CultureChangedEventArgs(name));
        }

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        public event CultureChangedEventHandler CultureChanged;

        #endregion

        #region Singleton

        private static MultiLangResourceManager _multiLangResourceManager;

        public static MultiLangResourceManager Instance => 
            _multiLangResourceManager ?? (_multiLangResourceManager = new MultiLangResourceManager());

        #endregion
    }

    public delegate void CultureChangedEventHandler(object sender, CultureChangedEventArgs e);

    public class CultureChangedEventArgs : EventArgs
    {
        public CultureChangedEventArgs(string cultureName)
        {
            CultureName = cultureName;
        }

        public string CultureName { get; private set; }
    }
}
