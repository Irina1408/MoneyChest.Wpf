using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Enums
{
    [Description("Event state")]
    public enum EventState
    {
        [Description("Active")]
        Active = 0,

        [Description("Paused")]
        Paused = 1,

        [Description("Closed")]
        Closed = 2
    }
}
