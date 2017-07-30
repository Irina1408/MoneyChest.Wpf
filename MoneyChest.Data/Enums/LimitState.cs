using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    [Description("Limit state")]
    public enum LimitState
    {
        [Description("Active")]
        Active = 0,

        [Description("Planned")]
        Planned = 1,

        [Description("Closed")]
        Closed = 2
    }
}
