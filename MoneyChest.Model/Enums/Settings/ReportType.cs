using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Enums
{
    public enum ReportType
    {
        [Description("Pie chart")]
        PieChart = 1,
        [Description("Table")]
        Table = 100
    }
}
