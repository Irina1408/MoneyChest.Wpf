using MahApps.Metro.Controls;
using MoneyChest.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MoneyChest.View.Main
{
    public class CustomHamburgerMenuItem : HamburgerMenuGlyphItem
    {
        public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(CustomHamburgerMenuItem), new PropertyMetadata(null));

        public Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public FrameworkElement View { get; set; }
    }
}
