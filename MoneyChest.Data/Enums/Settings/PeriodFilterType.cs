using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    public enum PeriodFilterType
    {
        [Description("Today")]
        Today,

        [Description("Yeasterday")]
        Yeasterday,

        [Description("This week")]
        ThisWeek,

        [Description("Previous week")]
        PreviousWeek,

        [Description("This month")]
        ThisMonth,

        [Description("Previous month")]
        PreviousMonth,

        [Description("This year")]
        ThisYear,

        [Description("Previous year")]
        PreviousYear,

        [Description("Custom period")]
        CustomPeriod,

        [Description("All")]
        All
    }
}
