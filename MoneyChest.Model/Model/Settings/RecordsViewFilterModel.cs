using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class RecordsViewFilterModel : IHasUserId
    {
        public RecordsViewFilterModel()
        {
            AllCategories = true;
            PeriodFilterType = PeriodFilterType.ThisMonth;

            CategoryIds = new List<int>();
        }

        public int UserId { get; set; }

        public bool AllCategories { get; set; }

        public string Description { get; set; }

        public string Remark { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public TransactionType? TransactionType { get; set; }
        
        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
