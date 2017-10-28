using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Model
{
    public class EventsScopeModel
    {
        public List<MoneyTransferEventModel> MoneyTransferEvents { get; set; } = new List<MoneyTransferEventModel>();
        public List<RepayDebtEventModel> RepayDebtEvents { get; set; } = new List<RepayDebtEventModel>();
        public List<SimpleEventModel> SimpleEvents { get; set; } = new List<SimpleEventModel>();

        public EventType GetEventType(int eventId)
        {
            if (MoneyTransferEvents.Any(_ => _.Id == eventId)) return EventType.MoneyTransfer;
            if (RepayDebtEvents.Any(_ => _.Id == eventId)) return EventType.RepayDebt;
            if (SimpleEvents.Any(_ => _.Id == eventId)) return EventType.Simple;

            throw new KeyNotFoundException("Event was not found");
        }

        public EventModel GetEvent(int eventId)
        {
            if (MoneyTransferEvents.Any(_ => _.Id == eventId)) return MoneyTransferEvents.First(_ => _.Id == eventId);
            if (RepayDebtEvents.Any(_ => _.Id == eventId)) return RepayDebtEvents.First(_ => _.Id == eventId);
            if (SimpleEvents.Any(_ => _.Id == eventId)) return SimpleEvents.First(_ => _.Id == eventId);

            return null;
        }
    }
}
