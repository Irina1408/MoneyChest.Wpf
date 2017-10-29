using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class DailyScheduleModel : ScheduleModel
    {
        public DailyScheduleModel() : base()
        {
            Period = 1;
            DateFrom = DateTime.Today.AddDays(1);
            ScheduleType = Data.Enums.ScheduleType.Daily;
        }

        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public int Period { get; set; }
    }
}
