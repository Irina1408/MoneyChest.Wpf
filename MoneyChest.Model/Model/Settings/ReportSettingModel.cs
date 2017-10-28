using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ReportSettingModel : IHasUserId
    {
        public int UserId { get; set; }

        public bool IncludeRecordsWithoutCategory { get; set; }

        public bool AllCategories { get; set; }

        public ReportType ReportType { get; set; }

        public TransactionType? DataType { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public int CategoryLevel { get; set; }
        
        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
