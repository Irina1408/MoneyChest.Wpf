using LiveCharts;
using LiveCharts.Definitions.Points;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Pages
{
    public class MCPieSeries : PieSeries
    {
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            if (label == "0") label = null;

            return base.GetPointView(point, label);
        }
    }

    public class MCStackedColumnSeries : StackedColumnSeries
    {
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            if (label == "0") label = null;

            return base.GetPointView(point, label);
        }
    }

    public class MCStackedRowSeries : StackedRowSeries
    {
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            if (label == "0") label = null;

            return base.GetPointView(point, label);
        }
    }
}
