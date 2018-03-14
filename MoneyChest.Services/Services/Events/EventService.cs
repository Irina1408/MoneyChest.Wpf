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
    }

    public interface IEventService : IEventService<EventModel>
    {
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

        #endregion

        #region Shared methods

        internal static Expression<Func<T, bool>> GetActiveEventsFilter<T>(int userId, DateTime dateFrom, DateTime dateUntil)
            where T: Evnt
        {
            return e => e.UserId == userId && e.EventState != EventState.Closed
                && (!e.PausedToDate.HasValue || e.PausedToDate.Value < dateUntil)
                && e.DateFrom <= dateUntil && (!e.DateUntil.HasValue || e.DateUntil >= dateFrom);
        }

        #endregion
    }
}
