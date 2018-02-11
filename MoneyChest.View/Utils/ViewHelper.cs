using MahApps.Metro.Controls;
using MoneyChest.View.Details;
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = resizable ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize
            };
        }

        public static void OpenDetailsWindow(this UserControl control, IEntityDetailsView detailsView, Action success = null)
        {
            // init window
            var window = control.InitializeDependWindow(false);
            // prepare window
            window.Content = detailsView;
            detailsView.PrepareParentWindow(window);
            // show window
            window.ShowDialog();
            if (detailsView.DialogResult)
            {
                success?.Invoke();
            }
        }
    }
}
