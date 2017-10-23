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

namespace MoneyChest.Services.Services.Events
{
    public interface IEventService
    {
        List<Evnt> Get(int userId, Expression<Func<Evnt, bool>> expression = null, params EventType[] eventTypes);
    }

    public class EventService : BaseService, IEventService
    {
        public EventService(ApplicationDbContext context) : base(context)
        {
        }
        
        public List<Evnt> Get(int userId, Expression<Func<Evnt, bool>> expression = null, params EventType[] eventTypes)
        {
            // check expression
            if (expression == null) expression = item => true;

            // check event types
            if(eventTypes == null || eventTypes.Length == 0)
                return GetAllTypes(userId, expression);

            var result = new List<Evnt>();

            foreach (var eventType in eventTypes.Distinct())
                result.AddRange(Get(userId, eventType, expression));

            return result;
        }

        public List<T> Get<T>(int userId, List<int> storageGroupIds)
            where T : Evnt
        {
            if(typeof(T) == typeof(SimpleEvent))
                return _context.SimpleEvents.Where(_ => _.UserId == userId && storageGroupIds.Contains(_.Storage.StorageGroupId))
                    .Select(_ => _ as T).ToList();

            if (typeof(T) == typeof(RepayDebtEvent))
                return _context.RepayDebtEvents.Where(_ => _.UserId == userId && storageGroupIds.Contains(_.Storage.StorageGroupId))
                    .Select(_ => _ as T).ToList();

            if (typeof(T) == typeof(MoneyTransferEvent))
                return _context.MoneyTransferEvents.Where(_ => _.UserId == userId 
                    && (storageGroupIds.Contains(_.StorageFrom.StorageGroupId) || storageGroupIds.Contains(_.StorageTo.StorageGroupId)))
                    .Select(_ => _ as T).ToList();

            if (typeof(T) == typeof(Evnt))
                return _context.Events.Where(_ => _.UserId == userId).Select(_ => _ as T).ToList();

            // else throw exception
            throw new ArgumentException($"Unknown event type: {typeof(T).Name}");
        }

        private List<Evnt> GetAllTypes(int userId, Expression<Func<Evnt, bool>> expression = null)
        {
            // check expression
            if (expression == null) expression = item => true;

            var result = new List<Evnt>();

            foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
                result.AddRange(Get(userId, eventType, expression));

            return result;
        }

        private List<Evnt> Get(int userId, EventType eventType, Expression<Func<Evnt, bool>> expression = null)
        {
            // check expression
            if (expression == null) expression = item => true;

            switch (eventType)
            {
                case EventType.Simple:
                    return _context.SimpleEvents.Where(_ => _.UserId == userId).Where(expression).Select(_ => _ as Evnt).ToList();

                case EventType.RepayDebt:
                    return _context.RepayDebtEvents.Where(_ => _.UserId == userId).Where(expression).Select(_ => _ as Evnt).ToList();

                case EventType.MoneyTransfer:
                    return _context.MoneyTransferEvents.Where(_ => _.UserId == userId).Where(expression).Select(_ => _ as Evnt).ToList();

                default:
                    return GetAllTypes(userId, expression);
            }
        }
        
        private List<T> Get<T>(int userId, Expression<Func<T, bool>> expression = null)
            where T : Evnt
        {
            // check expression
            if (expression == null) expression = item => true;

            if (typeof(T) == typeof(SimpleEvent))
                return _context.SimpleEvents.Where(_ => _.UserId == userId).Select(_ => _ as T).Where(expression).ToList();
            if (typeof(T) == typeof(RepayDebtEvent))
                return _context.RepayDebtEvents.Where(_ => _.UserId == userId).Select(_ => _ as T).Where(expression).ToList();
            if (typeof(T) == typeof(MoneyTransferEvent))
                return _context.MoneyTransferEvents.Where(_ => _.UserId == userId).Select(_ => _ as T).Where(expression).ToList();
            if (typeof(T) == typeof(Evnt))
                return _context.Events.Where(_ => _.UserId == userId).Select(_ => _ as T).Where(expression).ToList();

            // else throw exception
            throw new ArgumentException($"Unknown event type: {typeof(T).Name}");
        }
    }
}
