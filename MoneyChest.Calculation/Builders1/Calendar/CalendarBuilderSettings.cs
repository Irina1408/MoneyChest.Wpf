using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Calendar
{
    public class CalendarBuilderSettings
    {
        public DateTime From { get; set; }
        public DateTime Until { get; set; }
        public bool ShowLimits { get; set; }

        public void SetPeriod(int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth(year, month);

            From = new DateTime(year, month, 1);
            Until = new DateTime(year, month, daysInMonth);
        }
    }
}
