using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using System.Data.Entity;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using MoneyChest.Model.Enums;

namespace MoneyChest.Services.Services
{
    public interface ISimpleEventService : IIdManagableUserableListServiceBase<SimpleEventModel>
    {
        List<SimpleEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil);
    }

    public class SimpleEventService : HistoricizedIdManageableUserableListServiceBase<SimpleEvent, SimpleEventModel, SimpleEventConverter>, ISimpleEventService
    {
        public SimpleEventService(ApplicationDbContext context) : base(context)
        {
        }

        #region ISimpleEventService implementation

        //public List<SimpleEvent> GetActiveForDate(int userId, DateTime date, Expression<Func<Evnt, bool>> expression = null)
        //{
        //    // check expression
        //    if (expression == null) expression = item => true;

        //    // once events that should be applied in this day
        //    var onceEvents = (from evnt in _context.SimpleEvents.Include(_ => _.OnceSchedules)
        //                      join schedule in _context.OnceSchedules on evnt.Id equals schedule.EventId
        //                      where evnt.UserId == userId && schedule.Date == date 
        //                      && evnt.EventState != EventState.Closed
        //                      && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
        //                      select evnt).ToList();

        //    // daily events that should be applied in this day
        //    var dailyEvents = (from evnt in _context.SimpleEvents.Include(_ => _.DailySchedules)
        //                       join schedule in _context.DailySchedules on evnt.Id equals schedule.EventId
        //                       where evnt.UserId == userId 
        //                       && schedule.DateFrom >= date && schedule.DateUntil <= date
        //                       && (date - schedule.DateFrom).Days % schedule.Period == 0
        //                       && evnt.EventState != EventState.Closed
        //                       && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
        //                       select evnt).ToList();

        //    // weekly events that should be applied in this day
        //    var weeklyEvents = (from evnt in _context.SimpleEvents.Include(_ => _.WeeklySchedules)
        //                       join schedule in _context.WeeklySchedules on evnt.Id equals schedule.EventId
        //                       join week in _context.WeeklyScheduleDayOfWeeks on schedule.Id equals week.WeeklyScheduleId
        //                       where evnt.UserId == userId && week.DayOfWeek == date.DayOfWeek
        //                       && schedule.DateFrom >= date && schedule.DateUntil <= date
        //                       && evnt.EventState != EventState.Closed
        //                       && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
        //                       select evnt).Distinct().ToList();

        //    // monthly events that should be applied in this day
        //    var monthlyEvents = (from evnt in _context.SimpleEvents.Include(_ => _.MonthlySchedules)
        //                        join schedule in _context.MonthlySchedules on evnt.Id equals schedule.EventId
        //                        join month in _context.MonthlyScheduleMonths on schedule.Id equals month.MonthlyScheduleId
        //                        where evnt.UserId == userId && schedule.DayOfMonth == date.Day && month.Month == (Month)date.Month
        //                        && schedule.DateFrom >= date && schedule.DateUntil <= date
        //                        && evnt.EventState != EventState.Closed
        //                        && (!evnt.PausedToDate.HasValue || evnt.PausedToDate.Value < date)
        //                        select evnt).Distinct().ToList();

        //    return new List<SimpleEvent>();
        //}

        public List<SimpleEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            return Scope.Where(e => e.UserId == userId && e.EventState != EventState.Closed
                && (!e.PausedToDate.HasValue || e.PausedToDate.Value < dateUntil)
                && e.DateFrom <= dateUntil && (!e.DateUntil.HasValue || e.DateUntil >= dateFrom))
                .ToList().ConvertAll(_converter.ToModel);
        }

        //public SchedulesScopeModel GetActiveForDate(int userId, DateTime date)
        //{
        //    Expression<Func<Evnt, bool>> eventFilter = e => e.UserId == userId && e.EventState != EventState.Closed
        //        && (!e.PausedToDate.HasValue || e.PausedToDate.Value < date);

        //    var result = new SchedulesScopeModel();

        //    // once schedules that should be applied in this day
        //    result.OnceSchedules = (from evnt in _context.Events
        //                            join schedule in _context.OnceSchedules on evnt.Id equals schedule.EventId
        //                            where schedule.Date == date && eventFilter.Compile().Invoke(evnt)
        //                            select schedule).ToList().ConvertAll(_onceScheduleConverter.ToModel);

        //    // daily schedules that should be applied in this day
        //    result.DailySchedules = (from evnt in _context.Events
        //                             join schedule in _context.DailySchedules on evnt.Id equals schedule.EventId
        //                             where schedule.DateFrom >= date && schedule.DateUntil <= date
        //                             && (date - schedule.DateFrom).Days % schedule.Period == 0
        //                             && eventFilter.Compile().Invoke(evnt)
        //                             select schedule).ToList().ConvertAll(_dailyScheduleConverter.ToModel);

        //    // ?(period) weekly schedules that should be applied in this day
        //    result.WeeklySchedules = (from evnt in _context.Events
        //                              join schedule in _context.WeeklySchedules.Include(_ => _.WeeklyScheduleDaysOfWeek) on evnt.Id equals schedule.EventId
        //                              join week in _context.WeeklyScheduleDayOfWeeks on schedule.Id equals week.WeeklyScheduleId
        //                              where week.DayOfWeek == date.DayOfWeek
        //                              && schedule.DateFrom >= date && schedule.DateUntil <= date
        //                              && eventFilter.Compile().Invoke(evnt)
        //                              select schedule).Distinct().ToList().ConvertAll(_weeklyScheduleConverter.ToModel);

        //    // monthly schedules that should be applied in this day
        //    result.MonthlySchedules = (from evnt in _context.Events
        //                               join schedule in _context.MonthlySchedules.Include(_ => _.MonthlyScheduleMonths) on evnt.Id equals schedule.EventId
        //                               join month in _context.MonthlyScheduleMonths on schedule.Id equals month.MonthlyScheduleId
        //                               where schedule.DayOfMonth == date.Day && month.Month == (Month)date.Month
        //                               && eventFilter.Compile().Invoke(evnt)
        //                               select schedule).Distinct().ToList().ConvertAll(_monthlyScheduleConverter.ToModel);

        //    return result;
        //}

        #endregion

        #region Overrides

        protected override IQueryable<SimpleEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Currency).Include(_ => _.Category);

        #endregion
    }
}
