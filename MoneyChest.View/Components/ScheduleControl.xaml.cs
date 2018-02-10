using MoneyChest.Model.Enums;
using MoneyChest.Shared.MultiLang;
using MoneyChest.View.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl
    {
        private ObservableCollection<SelectableMultiLangEnumDescription> daysOfWeek;
        private ObservableCollection<SelectableMultiLangEnumDescription> months;
        private bool isPopulation;

        public ScheduleControl()
        {
            InitializeComponent();

            // load data
            daysOfWeek = MultiLangEnumHelper.ToSelectableCollection<DayOfWeek>();
            months = MultiLangEnumHelper.ToSelectableCollection<Month>();

            // fill schedule selectors
            comboScheduleTypes.ItemsSource = MultiLangEnumHelper.ToCollection(typeof(ScheduleType));
            comboDaysOfMonth.ItemsSource = ScheduleHelper.GetMonthes();
            DaysOfWeekControl.ItemsSource = daysOfWeek;
            MonthesControl.ItemsSource = months;
        }

        #region SelectedDaysOfWeek Property

        public ICollection<DayOfWeek> SelectedDaysOfWeek
        {
            get => (ICollection<DayOfWeek>)this.GetValue(SelectedDaysOfWeekProperty);
            set => this.SetValue(SelectedDaysOfWeekProperty, value);
        }

        public static readonly DependencyProperty SelectedDaysOfWeekProperty = DependencyProperty.Register(
            nameof(SelectedDaysOfWeek), typeof(ICollection<DayOfWeek>), typeof(ScheduleControl),
            new FrameworkPropertyMetadata(new Collection<DayOfWeek>(), SelectedDaysOfWeekChangedCallback));

        private static void SelectedDaysOfWeekChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // update selected items
            var s = (d as ScheduleControl);
            if (s.SelectedDaysOfWeek == null) return;

            s.isPopulation = true;
            
            foreach (var item in s.daysOfWeek)
                item.IsSelected = s.SelectedDaysOfWeek.Contains((DayOfWeek)item.Value);

            s.isPopulation = false;
        }

        #endregion

        #region SelectedMonthes Property

        public ICollection<Month> SelectedMonths
        {
            get => (ICollection<Month>)this.GetValue(SelectedMonthsProperty);
            set => this.SetValue(SelectedMonthsProperty, value);
        }

        public static readonly DependencyProperty SelectedMonthsProperty = DependencyProperty.Register(
            nameof(SelectedMonths), typeof(ICollection<Month>), typeof(ScheduleControl),
            new FrameworkPropertyMetadata(new Collection<Month>(), SelectedMonthsChangedCallback));

        private static void SelectedMonthsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // update selected items
            var s = (d as ScheduleControl);
            if (s.SelectedMonths == null) return;

            s.isPopulation = true;

            foreach (var item in s.months)
                item.IsSelected = s.SelectedMonths.Contains((Month)item.Value);

            s.isPopulation = false;
        }

        #endregion

        #region Event handlers

        private void DayOfWeek_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (isPopulation || SelectedDaysOfWeek == null) return;

            var chkBox = sender as CheckBox;
            if (chkBox is null) return;
            var item = chkBox.DataContext as SelectableMultiLangEnumDescription;
            if (item is null) return;

            if (chkBox.IsChecked.HasValue && chkBox.IsChecked.Value)
                SelectedDaysOfWeek.Add((DayOfWeek)item.Value);
            else if (chkBox.IsChecked.HasValue && !chkBox.IsChecked.Value)
                SelectedDaysOfWeek.Remove((DayOfWeek)item.Value);
        }

        private void Month_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (isPopulation || SelectedMonths == null) return;

            var chkBox = sender as CheckBox;
            if (chkBox is null) return;
            var item = chkBox.DataContext as SelectableMultiLangEnumDescription;
            if (item is null) return;

            if (chkBox.IsChecked.HasValue && chkBox.IsChecked.Value)
                SelectedMonths.Add((Month)item.Value);
            else if (chkBox.IsChecked.HasValue && !chkBox.IsChecked.Value)
                SelectedMonths.Remove((Month)item.Value);
        }

        #endregion
    }
}
