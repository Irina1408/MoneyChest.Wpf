using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    [Table(nameof(ReportSetting))]
    public class ReportSetting : IHasUserId
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }
        
        public bool ShowSettings { get; set; }
        public ChartType ChartType { get; set; }
        public RecordType DataType { get; set; }
        public int CategoryLevel { get; set; }
        public Sorting Sorting { get; set; }
        public bool ShowLegend { get; set; }
        public bool ShowValue { get; set; }
        public bool IncludeActualTransactions { get; set; }
        public bool IncludeFuturePlannedTransactions { get; set; }

        // Pie chart settings
        public int PieChartInnerRadius { get; set; }
        public int PieChartDetailsDepth { get; set; }

        // Bar chart settings
        public BarChartView BarChartView { get; set; }
        public BarChartSection BarChartSection { get; set; }
        public PeriodType BarChartSectionPeriod { get; set; }
        public bool BarChartDetail { get; set; }

        public int DataFilterId { get; set; }
        public int PeriodFilterId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(PeriodFilterId))]
        public virtual PeriodFilter PeriodFilter { get; set; }

        [ForeignKey(nameof(DataFilterId))]
        public virtual DataFilter DataFilter { get; set; }
    }
}
