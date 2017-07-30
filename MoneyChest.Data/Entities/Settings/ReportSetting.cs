using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class ReportSetting : SettingCategorized
    {
        public ReportSetting() : base()
        {
            DataType = TransactionType.Expense;
            PeriodFilterType = PeriodFilterType.ThisMonth;
            ReportType = ReportType.PieChart;
            CategoryLevel = -1;
        }

        public ReportType ReportType { get; set; }

        public TransactionType DataType { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public int CategoryLevel { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }

        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
