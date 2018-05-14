using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Pages
{
    public class ChartViewModel
    {
        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();
        public List<string> Titles { get; set; } = new List<string>();
        public string Total { get; set; }
    }
}
