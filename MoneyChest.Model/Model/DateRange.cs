using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DateRange
    {
        public DateRange()
        { }

        public DateRange(DateTime dateFrom, DateTime dateUntil)
        {
            DateFrom = dateFrom;
            DateUntil = dateUntil;
        }

        public DateTime DateFrom { get; set; }
        public DateTime DateUntil { get; set; }
    }
}
