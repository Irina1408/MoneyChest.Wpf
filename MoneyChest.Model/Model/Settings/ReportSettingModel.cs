using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ReportSettingModel : IHasUserId, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ReportSettingModel()
        {
            ChartType = ChartType.PieChart;
            CategoryLevel = -1;
            PeriodFilter = new PeriodFilterModel();
            DataFilter = new DataFilterModel();
        }

        public int UserId { get; set; }

        public bool ShowSettings { get; set; } = false;
        public ChartType ChartType { get; set; }
        public RecordType DataType { get; set; }
        public int CategoryLevel { get; set; }

        // Pie chart settings
        public int PieChartInnerRadius { get; set; }
        public int PieChartDetailsDepth { get; set; }

        // Bar chart settings
        public BarChartView BarChartView { get; set; }
        public BarChartSection BarChartSection { get; set; }
        public int BarChartCompareDepth { get; set; }
        public PeriodType BarChartSectionPeriod { get; set; }
        public bool BarChartCompareIncomeExpence { get; set; }
        public bool BarChartDetail { get; set; }

        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }
    }
}
