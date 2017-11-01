using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class MonthlyScheduleModel : ScheduleModel
    {
        public MonthlyScheduleModel() : base()
        {
            DateFrom = DateTime.Today.AddDays(1);
            ScheduleType = ScheduleType.Monthly;

            Months = new List<Month>();
        }

        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public int DayOfMonth { get; set; }

        public List<Month> Months { get; set; }
    }
}
