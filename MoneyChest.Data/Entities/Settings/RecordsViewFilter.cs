﻿using MoneyChest.Model.Base;
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
    [Table(nameof(RecordsViewFilter))]
    public class RecordsViewFilter : IHasUserId
    {
        // TODO: create RecordsViewSettings. Store there period, grouping
        public RecordsViewFilter()
        {
            Categories = new List<Category>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public bool AllCategories { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        [StringLength(4000)]
        public string Remark { get; set; }

        public PeriodFilterType PeriodFilterType { get; set; }

        public RecordType? RecordType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUntil { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
