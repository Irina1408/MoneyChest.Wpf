using MahApps.Metro.IconPacks;
using MoneyChest.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyChest.View.Utils
{
    public static class PageHelper
    {
        public static FrameworkElement GetPageIcon<T>(T page)
            where T : IPage
        {
            if (page is DashboardPage) return new PackIconMaterial() { Kind = PackIconMaterialKind.ViewDashboard };
            else if (page is TransactionsPage) return new PackIconModern() { Kind = PackIconModernKind.BookList };
            else if (page is CalendarPage) return new PackIconModern() { Kind = PackIconModernKind.CalendarMonth };
            else if (page is PlanningPage) return new PackIconMaterial() { Kind = PackIconMaterialKind.CalendarClock };
            else if (page is DebtsPage) return new PackIconModern() { Kind = PackIconModernKind.CalendarDollar };
            else if (page is ReportsPage) return new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.PieChart };
            else if (page is StoragesPage) return new PackIconMaterial() { Kind = PackIconMaterialKind.Bank };
            else if (page is CurrenciesPage) return new PackIconModern() { Kind = PackIconModernKind.CurrencyDollar };
            else if (page is CategoriesPage) return new PackIconEntypo() { Kind = PackIconEntypoKind.FlowTree };
            else if (page is SettingsPage) return new PackIconMaterial() { Kind = PackIconMaterialKind.Settings };
            else return null;
        }

        public static int GetPageOrder<T>(T page)
            where T : IPage
        {
            if (page is DashboardPage) return 1;
            else if (page is TransactionsPage) return 2;
            else if (page is CalendarPage) return 3;
            else if (page is PlanningPage) return 4;
            else if (page is DebtsPage) return 5;
            else if (page is ReportsPage) return 6;
            else if (page is StoragesPage) return 7;
            else if (page is CurrenciesPage) return 8;
            else if (page is CategoriesPage) return 9;
            else return 0;
        }
    }
}
