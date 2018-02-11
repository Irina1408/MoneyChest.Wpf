using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoneyChest.View.Pages
{
    public interface IPageViewOptions
    {
        string Label { get; }
        FrameworkElement Icon { get; }
        int Order { get; }
        bool ShowTopBorder { get; }
        FrameworkElement View { get; }
    }

    public interface IPageDataManagement
    {
        event EventHandler DataChanged;
        bool RequiresReload { get; set; }
        void Reload();
    }

    public interface IPage : IPageViewOptions, IPageDataManagement
    {
    }
}
