using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class EventModel : IHasId, IHasUserId
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public decimal Value { get; set; }

        public EventState EventState { get; set; }
        
        public DateTime? PausedToDate { get; set; }

        public bool AutoExecution { get; set; }
        
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }

        public EventType EventType { get; set; }

        public string Remark { get; set; }
        
        public int UserId { get; set; }
    }
}
