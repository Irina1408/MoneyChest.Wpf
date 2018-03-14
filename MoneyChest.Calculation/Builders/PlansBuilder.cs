using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders
{
    public class PlansBuilder
    {
        private IEventService _eventService;

        public PlansBuilder(IEventService eventService)
        {
            _eventService = eventService;
        }

        public List<PlannedTransactionModel<EventModel>> GetPlannedEvents(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            // do not load events if there are not future days in selection
            if(dateUntil <= DateTime.Today.AddDays(1).AddMilliseconds(-1)) return new List<PlannedTransactionModel<EventModel>>();
            
            // local variables
            var result = new List<PlannedTransactionModel<EventModel>>();
            // load events
            var events = _eventService.GetActiveForPeriod(userId, dateFrom, dateUntil);
            
            // loop for every future day in selection
            var date = dateFrom < DateTime.Today ? DateTime.Today : dateFrom.Date;
            while (date <= dateUntil)
            {
                // write events for this day
                WriteEvents(result, events, date);
                // next day
                date = date.AddDays(1);
            }

            return result;
        }

        private void WriteEvents(List<PlannedTransactionModel<EventModel>> plannedEvents, List<EventModel> events, DateTime date)
        {
            // write monthly events
            foreach (var evnt in events.Where(x => x.Schedule.ScheduleType == ScheduleType.Monthly && x.Schedule.DayOfMonth == date.Day
                                            && x.Schedule.Months.Contains((Month)date.Month)))
            {
                // write event
                plannedEvents.Add(new PlannedTransactionModel<EventModel>(evnt, date));
            }

            // write weekly events
            foreach (var evnt in events.Where(x => x.Schedule.ScheduleType == ScheduleType.Weekly && x.Schedule.DaysOfWeek.Contains(date.DayOfWeek)))
            {
                // write event
                plannedEvents.Add(new PlannedTransactionModel<EventModel>(evnt, date));
            }

            // write daily events
            foreach (var evnt in events.Where(x => x.Schedule.ScheduleType == ScheduleType.Daily && x.Schedule.Period > 0))
            {
                // check this event will in this day
                DateTime evntDate = evnt.DateFrom;
                while (evntDate < date)
                    evntDate = evntDate.AddDays(evnt.Schedule.Period);
                if (evntDate != date) continue;

                // write event
                plannedEvents.Add(new PlannedTransactionModel<EventModel>(evnt, date));
            }

            // write once events
            foreach (var evnt in events.Where(x => x.Schedule.ScheduleType == ScheduleType.Once 
                                            && x.DateFrom.Year == date.Year && x.DateFrom.Month == date.Month && x.DateFrom.Day == date.Day))
            {
                // write event
                plannedEvents.Add(new PlannedTransactionModel<EventModel>(evnt, date));
            }
        }
    }
}
