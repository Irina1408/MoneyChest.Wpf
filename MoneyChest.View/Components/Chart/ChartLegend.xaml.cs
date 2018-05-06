using LiveCharts.Wpf;
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

namespace MoneyChest.View.Components.Chart
{
    /// <summary>
    /// Interaction logic for ChartLegend.xaml
    /// </summary>
    public partial class ChartLegend : UserControl, IChartLegend
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ChartLegend()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
        }

        public List<SeriesViewModel> Series { get; set; }
        public bool ShowTotal => (Series?.Count ?? 0) > 0;

        #region Total Property

        public string Total
        {
            get => (string)this.GetValue(TotalProperty);
            set => this.SetValue(TotalProperty, value);
        }

        public static readonly DependencyProperty TotalProperty = DependencyProperty.Register(
            nameof(Total), typeof(string), typeof(ChartLegend));

        #endregion
    }
}
