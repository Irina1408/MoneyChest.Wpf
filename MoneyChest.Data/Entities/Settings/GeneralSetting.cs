using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities
{
    public class GeneralSetting : IHasUserId
    {
        public GeneralSetting()
        {
            Language = Language.English;
            FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        public Language Language { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
