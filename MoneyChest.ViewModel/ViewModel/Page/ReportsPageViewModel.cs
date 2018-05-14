using MoneyChest.Model.Model;
using MoneyChest.Model.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.ViewModel.Extensions;

namespace MoneyChest.ViewModel.ViewModel
{
    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class ReportsPageViewModel<TChartData>
        where TChartData: class, new()
    {
        public ReportSettingModel Settings { get; set; }
        public TChartData ChartData { get; set; } = new TChartData();
        public bool IsAnyData { get; set; }
        
        public ReportBuildSettings GetBuildSettings()
        {
            return new ReportBuildSettings()
            {
                DateFrom = Settings.PeriodFilter.DateFrom,
                DateUntil = Settings.PeriodFilter.DateUntil,
                CategoryLevel = Settings.CategoryLevel,
                DataType = Settings.DataType,
                Sorting = Settings.Sorting,
                ApplyFilter = Settings.DataFilter.ApplyFilter,
                DetailsDepth = Settings.ChartType == Model.Enums.ChartType.PieChart ? Settings.PieChartDetailsDepth : (Settings.BarChartDetail ? 1 : 0)
            };
        }
    }
}
