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
    public class GeneralSetting
    {
        public GeneralSetting()
        {
            Language = Language.English;
            HideCoinBoxStorages = true;
            FirstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        // Hide coin box accounts in every selection
        public bool HideCoinBoxStorages { get; set; }

        public Language Language { get; set; }

        public DayOfWeek FirstDayOfWeek { get; set; }

        public int DebtCategoryId { get; set; }

        // Money transfer comission category
        public int ComissionCategoryId { get; set; }


        [ForeignKey(nameof(DebtCategoryId))]
        public virtual Category DebtCategory { get; set; }
        
        [ForeignKey(nameof(ComissionCategoryId))]
        public virtual Category ComissionCategory { get; set; }


        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}
