using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyChest.View.Pages.DashboardItems
{
    public interface IDashboardItem
    {
        void Reload();
        Action ReloadActual { get; set; }
        bool ContainsActual { get; }
        FrameworkElement View { get; }
        int Order { get; }
    }
}
