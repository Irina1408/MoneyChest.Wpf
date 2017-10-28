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
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services
{
    public interface IScheduleService
    {
        SchedulesScopeModel GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil);
        SchedulesScopeModel GetActiveForDate(int userId, DateTime date);
    }

    public class ScheduleService : BaseService, IScheduleService
    {
        #region Private fields

        private OnceScheduleConverter _onceScheduleConverter;
        private DailyScheduleConverter _dailyScheduleConverter;
        private WeeklyScheduleConverter _weeklyScheduleConverter;
        private MonthlyScheduleConverter _monthlyScheduleConverter;

        #endregion

        #region Initialization

        public ScheduleService(ApplicationDbContext context) : base(context)
        {
            _onceScheduleConverter = new OnceScheduleConverter();
            _dailyScheduleConverter = new DailyScheduleConverter();
            _weeklyScheduleConverter = new WeeklyScheduleConverter();
            _monthlyScheduleConverter = new MonthlyScheduleConverter();
        }

        #endregion

        #region IScheduleService implementstion

        public SchedulesScopeModel GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            Expression<Func<Evnt, bool>> eventFilter = e => e.UserId == userId && e.EventState != EventState.Closed
                && (!e.PausedToDate.HasValue || e.PausedToDate.Value < dateUntil);

            var result = new SchedulesScopeModel();

            result.OnceSchedules = (from evnt in _context.Events
                                   join schedule in _context.OnceSchedules on evnt.Id equals schedule.EventId
                                   where schedule.Date >= dateFrom && schedule.Date <= dateUntil
                                   && eventFilter.Compile().Invoke(evnt)
                                   select schedule).ToList().ConvertAll(_onceScheduleConverter.ToModel);

            result.DailySchedules = (from evnt in _context.Events
                                     join schedule in _context.DailySchedules on evnt.Id equals schedule.EventId
                                     where schedule.DateFrom >= dateUntil 
                                     && (!schedule.DateUntil.HasValue || schedule.DateUntil <= dateFrom)
                                     && eventFilter.Compile().Invoke(evnt)
                                     select schedule).ToList().ConvertAll(_dailyScheduleConverter.ToModel);

            result.WeeklySchedules = (from evnt in _context.Events
                                      join schedule in _context.WeeklySchedules.Include(_ => _.WeeklyScheduleDaysOfWeek) on evnt.Id equals schedule.EventId
                                      join week in _context.WeeklyScheduleDayOfWeeks on schedule.Id equals week.WeeklyScheduleId
                                      where schedule.DateFrom >= dateUntil 
                                      && (!schedule.DateUntil.HasValue || schedule.DateUntil <= dateFrom)
                                      && eventFilter.Compile().Invoke(evnt)
                                      select schedule).Distinct().ToList().ConvertAll(_weeklyScheduleConverter.ToModel);

            result.MonthlySchedules = (from evnt in _context.Events
                                       join schedule in _context.MonthlySchedules.Include(_ => _.MonthlyScheduleMonths) on evnt.Id equals schedule.EventId
                                       join month in _context.MonthlyScheduleMonths on schedule.Id equals month.MonthlyScheduleId
                                       where schedule.DateFrom >= dateFrom 
                                       && (!schedule.DateUntil.HasValue || schedule.DateUntil <= dateFrom)
                                       && eventFilter.Compile().Invoke(evnt)
                                       select schedule).Distinct().ToList().ConvertAll(_monthlyScheduleConverter.ToModel);

            return result;
        }

        public SchedulesScopeModel GetActiveForDate(int userId, DateTime date)
        {
            Expression<Func<Evnt, bool>> eventFilter = e => e.UserId == userId && e.EventState != EventState.Closed
                && (!e.PausedToDate.HasValue || e.PausedToDate.Value < date);

            var result = new SchedulesScopeModel();

            // once schedules that should be applied in this day
            result.OnceSchedules = (from evnt in _context.Events
                                    join schedule in _context.OnceSchedules on evnt.Id equals schedule.EventId
                                    where schedule.Date == date && eventFilter.Compile().Invoke(evnt)
                                    select schedule).ToList().ConvertAll(_onceScheduleConverter.ToModel);

            // daily schedules that should be applied in this day
            result.DailySchedules = (from evnt in _context.Events
                                    join schedule in _context.DailySchedules on evnt.Id equals schedule.EventId
                                    where schedule.DateFrom >= date && schedule.DateUntil <= date
                                    && (date - schedule.DateFrom).Days % schedule.Period == 0
                                    && eventFilter.Compile().Invoke(evnt)
                                    select schedule).ToList().ConvertAll(_dailyScheduleConverter.ToModel);

            // ?(period) weekly schedules that should be applied in this day
            result.WeeklySchedules = (from evnt in _context.Events
                                    join schedule in _context.WeeklySchedules.Include(_ => _.WeeklyScheduleDaysOfWeek) on evnt.Id equals schedule.EventId
                                    join week in _context.WeeklyScheduleDayOfWeeks on schedule.Id equals week.WeeklyScheduleId
                                    where week.DayOfWeek == date.DayOfWeek
                                    && schedule.DateFrom >= date && schedule.DateUntil <= date
                                    && eventFilter.Compile().Invoke(evnt)
                                    select schedule).Distinct().ToList().ConvertAll(_weeklyScheduleConverter.ToModel);

            // monthly schedules that should be applied in this day
            result.MonthlySchedules = (from evnt in _context.Events
                                       join schedule in _context.MonthlySchedules.Include(_ => _.MonthlyScheduleMonths) on evnt.Id equals schedule.EventId
                                       join month in _context.MonthlyScheduleMonths on schedule.Id equals month.MonthlyScheduleId
                                       where schedule.DayOfMonth == date.Day && month.Month == (Month)date.Month
                                       && eventFilter.Compile().Invoke(evnt)
                                       select schedule).Distinct().ToList().ConvertAll(_monthlyScheduleConverter.ToModel);
            
            return result;
        }

        #endregion
    }
}
