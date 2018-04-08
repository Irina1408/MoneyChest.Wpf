using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Model;
using MoneyChest.Model.Enums;

namespace MoneyChest.Services.Services
{
    public interface ITransactionService
    {
        List<ITransaction> Get(int userId, DateTime dateFrom, DateTime dateUntil);
        List<ITransaction> GetActual(int userId, DateTime dateFrom, DateTime dateUntil);
        List<PlannedTransactionModel<EventModel>> GetPlanned(int userId, DateTime dateFrom, DateTime dateUntil);
        void Delete(IEnumerable<ITransaction> entities);
    }

    public class TransactionService : ServiceBase, ITransactionService
    {
        #region Private fields

        private IRecordService _recordService;
        private IMoneyTransferService _moneyTransferService;
        private IEventService _eventService;

        #endregion

        #region Initialization

        public TransactionService(ApplicationDbContext context) : base(context)
        {
            _recordService = new RecordService(context);
            _moneyTransferService = new MoneyTransferService(context);
            _eventService = new EventService(context);
        }

        #endregion

        #region ITransactionService implementation

        public List<ITransaction> Get(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<ITransaction>();

            // load actual data
            result.AddRange(GetActual(userId, dateFrom, dateUntil));
            // add events in case when selected period contains future dates
            if(dateUntil >= DateTime.Today)
                result.AddRange(GetPlanned(userId, dateFrom, dateUntil));

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public List<ITransaction> GetActual(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<ITransaction>();

            // load records
            result.AddRange(_recordService.Get(userId, dateFrom, dateUntil));
            // load money transfers
            result.AddRange(_moneyTransferService.Get(userId, dateFrom, dateUntil));

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public List<PlannedTransactionModel<EventModel>> GetPlanned(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            // do not load events if there are not future days in selection
            if (dateUntil <= DateTime.Today.AddDays(1).AddMilliseconds(-1)) return new List<PlannedTransactionModel<EventModel>>();

            // local variables
            var result = new List<PlannedTransactionModel<EventModel>>();
            // load events
            var events = _eventService.GetActiveForPeriod(userId, dateFrom, dateUntil);

            // loop for every future day in selection
            var date = dateFrom <= DateTime.Today ? DateTime.Today.AddDays(1) : dateFrom.Date;
            while (date <= dateUntil)
            {
                // write events for this day
                WriteEvents(result, events, date);
                // next day
                date = date.AddDays(1);
            }

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public void Delete(IEnumerable<ITransaction> entities)
        {
            // only actual transactions can be removed
            _recordService.Delete(entities.Where(x => x is RecordModel).Select(x => x as RecordModel));
            _moneyTransferService.Delete(entities.Where(x => x is MoneyTransferModel).Select(x => x as MoneyTransferModel));
        }

        #endregion

        #region Private methods

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

        #endregion
    }
}
