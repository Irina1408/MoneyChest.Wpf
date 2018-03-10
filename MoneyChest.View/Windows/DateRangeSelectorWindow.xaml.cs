using MahApps.Metro.Controls;
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

namespace MoneyChest.View.Windows
{
    /// <summary>
    /// Interaction logic for DateRangeSelectorWindow.xaml
    /// </summary>
    public partial class DateRangeSelectorWindow : MetroWindow
    {
        public DateRangeSelectorWindow()
        {
            InitializeComponent();
        }

        #region DateFrom Property

        public DateTime DateFrom
        {
            get => (DateTime)this.GetValue(DateFromProperty);
            set => this.SetValue(DateFromProperty, value);
        }

        public static readonly DependencyProperty DateFromProperty = DependencyProperty.Register(
            nameof(DateFrom), typeof(DateTime), typeof(DateRangeSelectorWindow));

        #endregion

        #region DateUntil Property

        public DateTime DateUntil
        {
            get => (DateTime)this.GetValue(DateUntilProperty);
            set => this.SetValue(DateUntilProperty, value);
        }

        public static readonly DependencyProperty DateUntilProperty = DependencyProperty.Register(
            nameof(DateUntil), typeof(DateTime), typeof(DateRangeSelectorWindow));

        #endregion

        #region Event handlers

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        #endregion
    }
}
