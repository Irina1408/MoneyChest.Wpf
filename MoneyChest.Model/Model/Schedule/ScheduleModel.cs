using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class ScheduleModel : IHasId
    {
        public int Id { get; set; }

        public ScheduleType ScheduleType { get; set; }

        public int EventId { get; set; }
    }
}
