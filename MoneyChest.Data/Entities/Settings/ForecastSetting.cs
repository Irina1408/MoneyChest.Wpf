﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class ForecastSetting
    {
        public ForecastSetting()
        {
            AllCategories = true;
            RepeatsCount = 5;
            ActualDays = 100;

            Categories = new List<Category>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public bool AllCategories { get; set; }

        [Description("Count of the minimum repeats")]
        public uint RepeatsCount { get; set; }

        public uint ActualDays { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
