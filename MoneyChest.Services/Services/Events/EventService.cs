using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;
using MoneyChest.Model.Enums;

namespace MoneyChest.Services.Services
{
    public interface IEventService<T>
            where T : EventModel
    {
        List<T> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil);
        List<T> GetNotClosed(int userId);
    }

    public interface IEventService : IEventService<EventModel>
    {
        void ExecuteEvents(int userId);
        bool UpdateEventState(EventModel model);
    }

    public class EventService : ServiceBase, IEventService
    {
        #region Private fields

        private ISimpleEventService _simpleEventService;
        private IMoneyTransferEventService _moneyTransferEventService;
        private IRepayDebtEventService _repayDebtEventService;

        #endregion

        #region Initialization

        public EventService(ApplicationDbContext context) : base(context)
        {
            _simpleEventService = new SimpleEventService(context);
            _repayDebtEventService = new RepayDebtEventService(context);
            _moneyTransferEventService = new MoneyTransferEventService(context);
        }

        #endregion

        #region IEventService implementation

        public List<EventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var result = new List<EventModel>();

            result.AddRange(_simpleEventService.GetActiveForPeriod(userId, dateFrom, dateUntil));
            result.AddRange(_repayDebtEventService.GetActiveForPeriod(userId, dateFrom, dateUntil));
            result.AddRange(_moneyTransferEventService.GetActiveForPeriod(userId, dateFrom, dateUntil));

            return result;
        }

        public List<EventModel> GetNotClosed(int userId)
        {
            var result = new List<EventModel>();

            result.AddRange(_simpleEventService.GetNotClosed(userId));
            result.AddRange(_repayDebtEventService.GetNotClosed(userId));
            result.AddRange(_moneyTransferEventService.GetNotClosed(userId));

            return result;
        }

        public void ExecuteEvents(int userId)
        {
            //var events = GetNotClosed(userId);
            
            //foreach(var evnt in events)
            //{
            //    if (UpdateEventState(evnt))
            //    {
            //        if (evnt is SimpleEventModel)
            //            _simpleEventService.Update(evnt as SimpleEventModel);
            //        else if (evnt is MoneyTransferEventModel)
            //            _moneyTransferEventService.Update(evnt as MoneyTransferEventModel);
            //        else if (evnt is RepayDebtEventModel)
            //            _repayDebtEventService.Update(evnt as RepayDebtEventModel);
            //    }
            //}
        }

        public bool UpdateEventState(EventModel model)
        {
            if (model.EventState == EventState.Paused && model.PausedToDate != null && model.PausedToDate <= DateTime.Today)
                model.EventState = EventState.Active;
            else
                return false;

            return true;
        }

        #endregion

        #region Shared methods

        internal static Expression<Func<T, bool>> GetActiveEventsFilter<T>(int userId, DateTime dateFrom, DateTime dateUntil)
            where T: Evnt
        {
            return e => e.UserId == userId && e.EventState != EventState.Closed
                && (!e.PausedToDate.HasValue || e.PausedToDate.Value < dateUntil)
                && e.DateFrom <= dateUntil && (!e.DateUntil.HasValue || e.DateUntil >= dateFrom);
        }

        internal static List<T> UpdateEventsExchangeRate<T>(ICurrencyExchangeRateService currencyExchangeRateService, List<T> events)
            where T : EventModel
        {
            var currencyExchangeRates = currencyExchangeRateService.GetList(events
                .Where(x => x.TakeExistingCurrencyExchangeRate && x.IsCurrencyExchangeRateRequired)
                .Select(x => new[] { x.CurrencyFromId, x.CurrencyToId })
                .SelectMany(x => x)
                .ToList());

            // update currency exchange rates for all events
            foreach (var evnt in events.Where(x => x.TakeExistingCurrencyExchangeRate && x.IsCurrencyExchangeRateRequired).ToList())
            {
                evnt.CurrencyExchangeRate = currencyExchangeRates.FirstOrDefault(x =>
                    x.CurrencyFromId == evnt.CurrencyFromId && x.CurrencyToId == evnt.CurrencyToId)?.Rate ?? 1;
            }

            return events;
        }

        #endregion
    }
}
