using MoneyChest.Calculation.Builders;
using MoneyChest.Model.Enums;
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

namespace MoneyChest.View.Components
{
    /// <summary>
    /// Interaction logic for CalendarDayControl.xaml
    /// </summary>
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public partial class CalendarDayControl : UserControl
    {
        public CalendarDayControl()
        {
            InitializeComponent();

            // init data context
            MainGrid.DataContext = this;
        }

        #region CalendarDayData Property

        public CalendarDayData Data
        {
            get => (CalendarDayData)this.GetValue(DataProperty);
            set => this.SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            nameof(Data), typeof(CalendarDayData), typeof(CalendarDayControl));

        #endregion

        #region Public properties

        // If shown day is today show "Today" else if shown day is first day of month show month name
        public string Header => Data != null && Data.IsToday 
            ? MultiLangResourceManager.Instance[MultiLangResourceName.Today] 
            : (Data != null && Data.DayOfMonth == 1 ? MultiLangResource.EnumItemDescription(typeof(Month), (Month)Data.Month) : "");

        #endregion
    }
}
