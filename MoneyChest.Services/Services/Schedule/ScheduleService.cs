using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Data.Entity;
using MoneyChest.Data.Enums;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services
{
    public interface IScheduleService
    {
        /// <summary>
        /// Loads all schedules that can be converted to specified schedule (schedule as OnceSchedule).
        /// Includes related events
        /// </summary>
        List<Schedule> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil);

        /// <summary>
        /// Loads schedules of special type.
        /// Includes related events
        /// </summary>
        List<T> GetActiveForPeriod<T>(int userId, DateTime dateFrom, DateTime dateUntil)
            where T : Schedule;
    }

    public class ScheduleService : BaseService, IScheduleService
    {
        public ScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        public List<Schedule> GetActiveForDate(int userId, DateTime date)
        {
            // once schedules that should be applied in this day
            var onceSchedules = (from evnt in _context.Events
                                 join schedule in _context.OnceSchedules on evnt.Id equals schedule.EventId
                                 where evnt.UserId == userId && schedule.Date == date
                                 && evnt.EventState != EventState.Closed
                                 && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
                                 select schedule).ToList();

            // daily schedules that should be applied in this day
            var dailySchedules = (from evnt in _context.Events
                                  join schedule in _context.DailySchedules on evnt.Id equals schedule.EventId
                                  where evnt.UserId == userId
                                  && schedule.DateFrom >= date && schedule.DateUntil <= date
                                  && (date - schedule.DateFrom).Days % schedule.Period == 0
                                  && evnt.EventState != EventState.Closed
                                  && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
                                  select schedule).ToList();

            // ?(period) weekly schedules that should be applied in this day
            var weeklySchedules = (from evnt in _context.Events
                                   join schedule in _context.WeeklySchedules on evnt.Id equals schedule.EventId
                                   join week in _context.WeeklyScheduleDayOfWeeks on schedule.Id equals week.WeeklyScheduleId
                                   where evnt.UserId == userId && week.DayOfWeek == date.DayOfWeek
                                   && schedule.DateFrom >= date && schedule.DateUntil <= date
                                   && evnt.EventState != EventState.Closed
                                   && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
                                   select schedule).Distinct().ToList();

            // monthly schedules that should be applied in this day
            var monthlySchedules = (from evnt in _context.Events
                                    join schedule in _context.MonthlySchedules on evnt.Id equals schedule.EventId
                                    join month in _context.MonthlyScheduleMonths on schedule.Id equals month.MonthlyScheduleId
                                    where evnt.UserId == userId && schedule.DayOfMonth == date.Day && month.Month == (Month)date.Month
                                    && schedule.DateFrom >= date && schedule.DateUntil <= date
                                    && evnt.EventState != EventState.Closed
                                    && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
                                    select schedule).Distinct().ToList();

            // fill result list
            var result = new List<Schedule>();
            result.AddRange(onceSchedules.Select(_ => _ as Schedule));
            result.AddRange(dailySchedules.Select(_ => _ as Schedule));
            result.AddRange(weeklySchedules.Select(_ => _ as Schedule));
            result.AddRange(monthlySchedules.Select(_ => _ as Schedule));

            return result;
        }

        /// <summary>
        /// Loads all schedules that can be converted to specified schedule (schedule as OnceSchedule).
        /// Includes related events
        /// </summary>
        public List<Schedule> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            // fill result list
            var result = new List<Schedule>();
            result.AddRange(GetActiveForPeriod<OnceSchedule>(userId, dateFrom, dateUntil).Select(_ => _ as Schedule));
            result.AddRange(GetActiveForPeriod<DailySchedule>(userId, dateFrom, dateUntil).Select(_ => _ as Schedule));
            result.AddRange(GetActiveForPeriod<WeeklySchedule>(userId, dateFrom, dateUntil).Select(_ => _ as Schedule));
            result.AddRange(GetActiveForPeriod<MonthlySchedule>(userId, dateFrom, dateUntil).Select(_ => _ as Schedule));

            return result;
        }

        public List<T> GetActiveForPeriod<T>(int userId, DateTime dateFrom, DateTime dateUntil)
            where T : Schedule
        {
            Expression<Func<Evnt, bool>> expressionEvent = e => e.UserId == userId && e.EventState != EventState.Closed
                && (!e.PausedToDate.HasValue || e.PausedToDate.Value < dateUntil);

            // return once schedules
            if (typeof(T) == typeof(OnceSchedule))
                return (from evnt in _context.Events
                 join schedule in _context.OnceSchedules.Include(_ => _.Event)
                                                        .Include(_ => _.SimpleEvent)
                                                        .Include(_ => _.MoneyTransferEvent)
                                                        .Include(_ => _.RepayDebtEvent)
                                                        on evnt.Id equals schedule.EventId
                 where schedule.Date >= dateFrom && schedule.Date <= dateUntil
                 && expressionEvent.Compile().Invoke(evnt)
                 select schedule as T).ToList();

            // ? (period) return daily schedules
            if (typeof(T) == typeof(DailySchedule))
                return (from evnt in _context.Events
                        join schedule in _context.DailySchedules.Include(_ => _.Event)
                                                              .Include(_ => _.SimpleEvent)
                                                              .Include(_ => _.MoneyTransferEvent)
                                                              .Include(_ => _.RepayDebtEvent)
                                                              on evnt.Id equals schedule.EventId
                        where schedule.DateFrom >= dateUntil && schedule.DateUntil <= dateFrom
                        && expressionEvent.Compile().Invoke(evnt)
                        select schedule as T).ToList();

            // ? (period, day of week) return weekly schedules
            if (typeof(T) == typeof(WeeklySchedule))
                return (from evnt in _context.Events
                        join schedule in _context.WeeklySchedules.Include(_ => _.Event)
                                                             .Include(_ => _.SimpleEvent)
                                                             .Include(_ => _.MoneyTransferEvent)
                                                             .Include(_ => _.RepayDebtEvent)
                                                             .Include(_ => _.WeeklyScheduleDaysOfWeek)
                                                             on evnt.Id equals schedule.EventId
                        join week in _context.WeeklyScheduleDayOfWeeks on schedule.Id equals week.WeeklyScheduleId
                        where schedule.DateFrom >= dateUntil && schedule.DateUntil <= dateFrom
                        && expressionEvent.Compile().Invoke(evnt)
                        select schedule as T).Distinct().ToList();

            // ? (day of month, month) return monthly schedules
            if (typeof(T) == typeof(MonthlySchedule))
                return (from evnt in _context.Events
                        join schedule in _context.MonthlySchedules.Include(_ => _.Event)
                                                            .Include(_ => _.SimpleEvent)
                                                            .Include(_ => _.MoneyTransferEvent)
                                                            .Include(_ => _.RepayDebtEvent)
                                                            .Include(_ => _.MonthlyScheduleMonths)
                                                            on evnt.Id equals schedule.EventId
                        join month in _context.MonthlyScheduleMonths on schedule.Id equals month.MonthlyScheduleId
                        where schedule.DateFrom >= dateFrom && schedule.DateUntil <= dateFrom
                        && expressionEvent.Compile().Invoke(evnt)
                        select schedule as T).Distinct().ToList();

            // else throw exception
            throw new ArgumentException($"Unknown schedule type: {typeof(T).Name}");
        }
    }
}
