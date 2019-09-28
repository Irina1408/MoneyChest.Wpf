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

        public bool ShowSettings { get; set; } = true;
        public ChartType ChartType { get; set; }
        public RecordType DataType { get; set; }
        public int CategoryLevel { get; set; }
        public Sorting Sorting { get; set; }
        public bool ShowLegend { get; set; } = true;
        public bool ShowValue { get; set; } = true;
        public bool IncludeActualTransactions { get; set; } = true;
        public bool IncludeFuturePlannedTransactions { get; set; } = false;

        // Pie chart settings
        public int PieChartInnerRadius { get; set; } = 150;
        public int PieChartDetailsDepth { get; set; }

        // Bar chart settings
        public BarChartView BarChartView { get; set; } = BarChartView.Vertical;
        public BarChartSection BarChartSection { get; set; }
        public PeriodType BarChartSectionPeriod { get; set; }
        public bool BarChartDetail { get; set; }

        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }

        // view helper properties
        public bool IsPieChartSelected => ChartType == ChartType.PieChart;
        public bool IsBarChartSelected => ChartType == ChartType.BarChart;
        public bool IsBarChartColumnsSelected => IsBarChartSelected && BarChartView == BarChartView.Vertical;
        public bool IsBarChartRowsSelected => IsBarChartSelected && BarChartView == BarChartView.Horizontal;
        public bool ShowPercentage => IsPieChartSelected || (BarChartSection == BarChartSection.Category && BarChartDetail);
        public bool ShowLegendView => ShowLegend && (IsPieChartSelected || (BarChartSection == BarChartSection.Category));

        // enable/disable properties
        public bool IsCategoryLevelEnabled => IsPieChartSelected || BarChartSection == BarChartSection.Category;
        public bool IsSortingEnabled => IsPieChartSelected || BarChartSection == BarChartSection.Category;
        public bool IsBarChartPeriodTypeEnabled => IsBarChartSelected && BarChartSection == BarChartSection.Period;
        public bool IsBarChartDetailEnabled => IsBarChartSelected && BarChartSection == BarChartSection.Category;
    }
}
