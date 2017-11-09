using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MoneyChest.View.Utils
{
    public static class ViewHelper
    {
        public static MetroWindow InitializeDependWindow(this UserControl control, bool resizable = true)
        {
            return new MetroWindow()
            {
                Style = Application.Current.FindResource("MoneyChestWindowStyle") as System.Windows.Style,
                Owner = Window.GetWindow(control),
                ResizeMode = resizable ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize
            };
        }
    }
}
