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
    /// Interaction logic for DateSelectorWindow.xaml
    /// </summary>
    public partial class DateSelectorWindow : MetroWindow
    {
        public DateSelectorWindow()
        {
            InitializeComponent();
        }

        #region Date Property

        public DateTime Date
        {
            get => (DateTime)this.GetValue(DateProperty);
            set => this.SetValue(DateProperty, value);
        }

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            nameof(Date), typeof(DateTime), typeof(DateSelectorWindow));

        #endregion

        #region Caption Property

        public string Caption
        {
            get => (string)this.GetValue(CaptionProperty);
            set => this.SetValue(CaptionProperty, value);
        }

        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(
            nameof(Caption), typeof(string), typeof(DateSelectorWindow));

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
