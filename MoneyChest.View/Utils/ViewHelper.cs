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
                Style = Application.Current.FindResource("MCWindowStyle") as System.Windows.Style,
                Owner = Window.GetWindow(control),
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = resizable ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize
            };
        }

        public static void OpenDependWindow(this UserControl control, object content, bool resizable = true)
        {
            // init
            var window = control.InitializeDependWindow(resizable);
            // prepare and show
            window.Content = content;
            window.ShowDialog();
        }

        public static bool ShowDateRangeSelector(this UserControl control, ref DateTime dateFrom, ref DateTime dateUntil)
        {
            var dateRangeSeletor = new MoneyChest.View.Windows.DateRangeSelectorWindow();
            dateRangeSeletor.Owner = Window.GetWindow(control);
            dateRangeSeletor.DateFrom = dateFrom;
            dateRangeSeletor.DateUntil = dateUntil;

            if(dateRangeSeletor.ShowDialog() == true)
            {
                dateFrom = dateRangeSeletor.DateFrom;
                dateUntil = dateRangeSeletor.DateUntil;
                return true;
            }

            return false;
        }

        public static bool ShowDateSelector(this UserControl control, ref DateTime date, string caption)
        {
            var dateSeletor = new MoneyChest.View.Windows.DateSelectorWindow();
            dateSeletor.Owner = Window.GetWindow(control);
            dateSeletor.Date = date;
            dateSeletor.Caption = caption;

            if (dateSeletor.ShowDialog() == true)
            {
                date = dateSeletor.Date;
                return true;
            }

            return false;
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
