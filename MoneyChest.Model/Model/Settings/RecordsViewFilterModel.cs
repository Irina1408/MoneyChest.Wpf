﻿using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public RecordType? RecordType { get; set; }
        
        public DateTime? DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
