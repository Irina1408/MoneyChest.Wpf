using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ReportSettingModel : IHasUserId
    {
        public ReportSettingModel()
        {
            PeriodFilterType = PeriodFilterType.ThisMonth;
            ReportType = ReportType.PieChart;
            CategoryLevel = -1;
            AllCategories = true;
            IncludeRecordsWithoutCategory = true;

            CategoryIds = new List<int>();
        }

        public int UserId { get; set; }

        public bool IncludeRecordsWithoutCategory { get; set; }

        public bool AllCategories { get; set; }

        public ReportType ReportType { get; set; }

        public RecordType? DataType { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public int CategoryLevel { get; set; }
        
        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
