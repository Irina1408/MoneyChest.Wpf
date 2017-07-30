using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class ForecastSetting : SettingCategorized
    {
        public ForecastSetting()
        {
            RepeatsCount = 5;
            ActualDays = 100;
        }

        [Description("Count of the minimum repeats")]
        public uint RepeatsCount { get; set; }

        public uint ActualDays { get; set; }

        [Required]
        public int UserId { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
