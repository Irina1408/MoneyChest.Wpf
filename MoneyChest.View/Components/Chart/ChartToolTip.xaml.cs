using LiveCharts;
using LiveCharts.Defaults;
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
    /// Interaction logic for ChartToolTip.xaml
    /// </summary>
    public partial class ChartToolTip : UserControl, IChartTooltip
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ChartToolTip()
        {
            InitializeComponent();
            MainGrid.DataContext = this;
        }

        public TooltipData Data { get; set; }
        public TooltipSelectionMode? SelectionMode { get; set; }
        public Series Sender => Data?.SenderSeries;
        public double? Value => (Sender?.Values as ChartValues<ObservableValue>)?.Where(x => x.Value != 0)?.FirstOrDefault()?.Value;
    }
}
