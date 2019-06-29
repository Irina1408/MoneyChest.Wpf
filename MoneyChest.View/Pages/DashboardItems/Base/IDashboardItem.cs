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
        FrameworkElement View { get; }
        int Order { get; }
    }
}
