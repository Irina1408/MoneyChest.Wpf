using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoneyChest.View.Pages
{
    public interface IPage
    {
        string Label { get; }
        FrameworkElement Icon { get; }
        int Order { get; }
        bool ShowTopBorder { get; }
        FrameworkElement View { get; }
    }
}
