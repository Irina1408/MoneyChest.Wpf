﻿using MahApps.Metro.Controls;
using MoneyChest.View.Pages;
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
using System.Windows.Shapes;

namespace MoneyChest.View.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Event handlers

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // get all application pages
            var pages = new List<IPage>();
            System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(mytype => mytype.GetInterfaces().Contains(typeof(IPage)) && !mytype.IsAbstract)
                .ToList()
                .ForEach(t => pages.Add(Activator.CreateInstance(t) as IPage));

            // build hamburger menu
            foreach (var page in pages.OrderBy(_ => _.Order))
            {
                // add menu item into the menu
                HamburgerMenuControl.Items.Add(new CustomHamburgerMenuItem()
                {
                    Label = page.Label,
                    Tag = page.Icon,
                    BorderThickness = page.ShowTopBorder ? new Thickness(0, 1, 0, 0) : new Thickness(),
                    View = page.View
                });

                // add events
                page.DataChanged += (s, arg) => pages.ForEach(p =>
                {
                    if (p != page)
                        p.RequiresReload = true;
                });
            }
        }

        #endregion
    }
}