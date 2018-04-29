using MoneyChest.Calculation.Builders;
using MoneyChest.Model.Calendar;
using MoneyChest.Model.Enums;
using MoneyChest.Shared.MultiLang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class CalendarDayControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
            nameof(Data), typeof(CalendarDayData), typeof(CalendarDayControl), new FrameworkPropertyMetadata(null, DataChangedCallback));

        private static void DataChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get control
            var control = (d as CalendarDayControl);
            if(control.Data != null)
                control.Data.PropertyChanged += (sender, arg) =>
                {
                    if (arg.PropertyName == nameof(CalendarDayData.IsLimitedTransactions))
                        control.PropertyChanged?.Invoke(control, new PropertyChangedEventArgs(nameof(ShowDots)));
                };
        }

        #endregion

        #region Public properties

        // If shown day is today show "Today" else if shown day is first day of month show month name
        public string Header => Data != null && Data.IsToday 
            ? MultiLangResourceManager.Instance[MultiLangResourceName.Today] 
            : (Data != null && Data.DayOfMonth == 1 ? MultiLangResource.EnumItemDescription(typeof(Month), (Month)Data.Month) : "");

        public bool IsActive => Data != null;
        public bool ShowDots => Data?.IsLimitedTransactions ?? false;
        public bool ShowAnyAccountNegative => (Data?.IsAnyAccountNegative ?? false) && !ShowAllStorages;
        public bool ShowAllStorages { get; set; }

        #endregion
    }
}
