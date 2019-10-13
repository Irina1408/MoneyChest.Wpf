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
        List<ITransaction> GetActual(int userId, DateTime dateFrom, DateTime dateUntil, bool? AutoExecuted = null);
        List<ITransaction> GetActual(int userId, DateTime dateFrom, DateTime dateUntil, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null);
        List<PlannedTransactionModel<EventModel>> GetPlanned(int userId, DateTime dateFrom, DateTime dateUntil, bool onlyFuture = true, bool? autoExecution = null);

        List<ITransaction> ExecutePlanned(IEnumerable<ITransaction> transactions, DateTime? date = null, bool isAutoExecution = false);
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
            if(dateUntil >= DateTime.Today.AddDays(1))
                result.AddRange(GetPlanned(userId, dateFrom, dateUntil));

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public List<ITransaction> GetActual(int userId, DateTime dateFrom, DateTime dateUntil, bool? AutoExecuted = null)
        {
            var result = new List<ITransaction>();

            // load records
            result.AddRange(_recordService.Get(userId, dateFrom, dateUntil, AutoExecuted));
            // load money transfers
            result.AddRange(_moneyTransferService.Get(userId, dateFrom, dateUntil, AutoExecuted));

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public List<ITransaction> GetActual(int userId, DateTime dateFrom, DateTime dateUntil, RecordType recordType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            var result = new List<ITransaction>();

            // load records
            result.AddRange(_recordService.Get(userId, dateFrom, dateUntil, recordType, includeWithoutCategory, categoryIds));
            // load money transfers
            result.AddRange(_moneyTransferService.Get(userId, dateFrom, dateUntil, recordType, includeWithoutCategory, categoryIds));

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public List<PlannedTransactionModel<EventModel>> GetPlanned(int userId, DateTime dateFrom, DateTime dateUntil,
            bool onlyFuture = true, bool? autoExecution = null)
        {
            // prepare
            var date = onlyFuture && dateFrom.Date <= DateTime.Today ? DateTime.Today.AddDays(1) : dateFrom.Date;

            // do not load events if there are not future days in selection if onlyFuture
            if (dateUntil <= date.AddMilliseconds(-1)) return new List<PlannedTransactionModel<EventModel>>();

            // local variables
            var result = new List<PlannedTransactionModel<EventModel>>();
            // load events
            var events = _eventService.GetActiveForPeriod(userId, dateFrom, dateUntil, autoExecution);

            // loop for every day in selection
            while (date <= dateUntil)
            {
                // write events for this day
                WriteEvents(result, events.Where(x => (x.ActualEventState != ActualEventState.Paused || x.PausedToDate <= date)
                    && x.DateFrom <= date && (x.DateUntil == null || x.DateUntil > date)).ToList(), date);
                // next day
                date = date.AddDays(1);
            }

            return result.OrderByDescending(x => x.TransactionDate).ToList();
        }

        public List<ITransaction> ExecutePlanned(IEnumerable<ITransaction> transactions, DateTime? date = null, bool isAutoExecution = false)
        {
            var result = new List<ITransaction>();

            foreach (var transaction in transactions)
            {
                var plannedTransaction = transaction as PlannedTransactionModel<EventModel>;
                if (plannedTransaction == null) continue;

                // simple event
                if (plannedTransaction.Event is SimpleEventModel)
                    result.Add(_recordService.Add(_recordService.Create(plannedTransaction.Event as SimpleEventModel, x =>
                    {
                        x.Date = date ?? plannedTransaction.TransactionDate;
                        x.IsAutoExecuted = isAutoExecution;
                    })));

                // repay debt
                if (plannedTransaction.Event is RepayDebtEventModel)
                    result.Add(_recordService.Add(_recordService.Create(plannedTransaction.Event as RepayDebtEventModel, x =>
                    {
                        x.Date = date ?? plannedTransaction.TransactionDate;
                        x.IsAutoExecuted = isAutoExecution;
                    })));

                // money transfer
                if (plannedTransaction.Event is MoneyTransferEventModel)
                    result.Add(_moneyTransferService.Add(_moneyTransferService.Create(plannedTransaction.Event as MoneyTransferEventModel, x =>
                    {
                        x.Date = date ?? plannedTransaction.TransactionDate;
                        x.IsAutoExecuted = isAutoExecution;
                    })));
            }

            return result;
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
            var lastDayOfMonth = DateTime.DaysInMonth(date.Year, date.Month);

            // write monthly/weekly/once events
            // TODO: check weekly period
            foreach (var evnt in events.Where(x => SuitableMonthlyEvent(x, date, lastDayOfMonth) 
                                                || SuitableWeeklyEvent(x, date)
                                                || SuitableOnceEvent(x, date)))
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
        }

        private bool SuitableMonthlyEvent(EventModel evnt, DateTime date, int lastDayOfMonth) => 
            evnt.Schedule.ScheduleType == ScheduleType.Monthly
            && evnt.Schedule.Months.Contains((Month)date.Month)
            && (evnt.Schedule.DayOfMonth == date.Day || evnt.Schedule.DayOfMonth == -1 && date.Day == lastDayOfMonth);

        private bool SuitableWeeklyEvent(EventModel evnt, DateTime date) => evnt.Schedule.ScheduleType == ScheduleType.Weekly 
            && evnt.Schedule.DaysOfWeek.Contains(date.DayOfWeek);

        private bool SuitableOnceEvent(EventModel evnt, DateTime date) => evnt.Schedule.ScheduleType == ScheduleType.Once
            && evnt.DateFrom.Year == date.Year && evnt.DateFrom.Month == date.Month && evnt.DateFrom.Day == date.Day;


        #endregion
    }
}
