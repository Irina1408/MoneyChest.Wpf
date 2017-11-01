using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Enums;

namespace MoneyChest.Model.Model
{
    public class OnceScheduleModel : ScheduleModel
    {
        public OnceScheduleModel() : base()
        {
            Date = DateTime.Today.AddDays(1);
            ScheduleType = ScheduleType.Once;
        }

        public DateTime Date { get; set; }
    }
}
