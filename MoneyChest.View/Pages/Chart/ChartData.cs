using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Pages
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ChartData
    {
        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public ColorsCollection ColorsCollection { get; set; } = new ColorsCollection();
        public List<string> Titles { get; set; } = new List<string>();
        public string Total { get; set; }
    }
}
