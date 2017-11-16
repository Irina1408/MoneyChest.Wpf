﻿using MahApps.Metro.IconPacks;
using MoneyChest.Shared.MultiLang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MoneyChest.View.Pages
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : UserControl, IPage
    {
        public CalendarPage()
        {
            InitializeComponent();
        }

        #region IPage implementation

        public string Label => MultiLangResourceManager.Instance[MultiLangResourceName.Calendar];
        public FrameworkElement Icon { get; private set; } = new PackIconModern() { Kind = PackIconModernKind.CalendarMonth };
        public int Order => 3;
        public bool ShowTopBorder => false;
        public FrameworkElement View => this;

        #endregion
    }
}
