using MoneyChest.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.ViewModel.ViewModel
{
    public class MainViewModel
    {
        public bool IsMenuHeadersShown
        {
            get => AppSettings.Instance.IsMenuHeadersShown;
            set
            {
                AppSettings.Instance.IsMenuHeadersShown = value;
                AppSettings.Instance.Save();
            }
        }
    }
}
