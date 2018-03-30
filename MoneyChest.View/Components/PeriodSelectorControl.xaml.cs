using MoneyChest.Model.Model;
using MoneyChest.View.Utils;
using MoneyChest.ViewModel.Commands;
using MoneyChest.ViewModel.Extensions;
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
    /// Interaction logic for PeriodSelectorControl.xaml
    /// </summary>
    public partial class PeriodSelectorControl : UserControl
    {
        public PeriodSelectorControl()
        {
            InitializeComponent();

            // init commands
            PrevDateRangeCommand = new Command(() => PeriodFilter.PrevDateRange());
            NextDateRangeCommand = new Command(() => PeriodFilter.NextDateRange());
            SelectDateRangeCommand = new Command(() =>
            {
                var dateFrom = PeriodFilter.DateFrom;
                var dateUntil = PeriodFilter.DateUntil;
                if (this.ShowDateRangeSelector(ref dateFrom, ref dateUntil))
                {
                    PeriodFilter.DateFrom = dateFrom;
                    PeriodFilter.DateUntil = dateUntil;
                }
            });

            MainPanel.DataContext = this;
        }

        #region PeriodFilter Property

        public PeriodFilterModel PeriodFilter
        {
            get => (PeriodFilterModel)this.GetValue(PeriodFilterProperty);
            set => this.SetValue(PeriodFilterProperty, value);
        }

        public static readonly DependencyProperty PeriodFilterProperty = DependencyProperty.Register(
            nameof(PeriodFilter), typeof(PeriodFilterModel), typeof(PeriodSelectorControl));

        #endregion

        #region Public properties

        public IMCCommand PrevDateRangeCommand { get; private set; }
        public IMCCommand NextDateRangeCommand { get; private set; }
        public IMCCommand SelectDateRangeCommand { get; private set; }

        #endregion
    }
}
