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
            ReportType = ReportType.PieChart;
            CategoryLevel = -1;
            PeriodFilter = new PeriodFilterModel();
            DataFilter = new DataFilterModel();
        }

        public int UserId { get; set; }

        public bool ShowSettings { get; set; } = true;
        public ReportType ReportType { get; set; }

        public RecordType? DataType { get; set; }
        public int CategoryLevel { get; set; }

        public PeriodFilterModel PeriodFilter { get; set; }
        public DataFilterModel DataFilter { get; set; }
    }
}
