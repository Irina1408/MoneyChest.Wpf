using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MonthlyScheduleModel : ScheduleModel
    {
        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public int DayOfMonth { get; set; }

        public List<Month> Months { get; set; } = new List<Month>();
    }
}
