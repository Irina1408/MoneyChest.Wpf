using MoneyChest.Model.Base;
using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class EventModel : IHasId, IHasUserId
    {
        public EventModel()
        {
            EventState = EventState.Active;
            AutoExecution = false;
            ConfirmBeforeExecute = false;
        }

        public int Id { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public decimal Value { get; set; }

        public EventState EventState { get; set; }

        public EventType EventType { get; set; }


        public ScheduleModel Schedule { get; set; }
        
        public DateTime DateFrom { get; set; }
        
        public DateTime? DateUntil { get; set; }

        public DateTime? PausedToDate { get; set; }

        public bool AutoExecution { get; set; }
        
        public TimeSpan? AutoExecutionTime { get; set; }

        public bool ConfirmBeforeExecute { get; set; }


        [StringLength(4000)]
        public string Remark { get; set; }
        
        public int UserId { get; set; }
    }
}
