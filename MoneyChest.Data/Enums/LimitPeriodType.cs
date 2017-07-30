using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    [Description("Limit period type")]
    public enum LimitPeriodType
    {
        [Description("Day")]
        Day = 0,

        [Description("Week")]
        Week = 1,

        [Description("Month")]
        Month = 2,

        [Description("Year")]
        Year = 3
    }
}
