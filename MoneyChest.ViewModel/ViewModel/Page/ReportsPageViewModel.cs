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
    public class ReportsPageViewModel<TSpecial>
        where TSpecial: class, new()
    {
        public ReportSettingModel Settings { get; set; }
        public TSpecial Special { get; set; } = new TSpecial();
        public string Total { get; set; }
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
                DetailsDepth = Settings.PieChartDetailsDepth
            };
        }
    }
}
