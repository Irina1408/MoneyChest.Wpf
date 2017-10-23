using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services
{
    public interface IWeeklyScheduleService : IBaseIdManagableService<WeeklyScheduleModel>
    {
    }

    public class WeeklyScheduleService : BaseHistoricizedIdManageableService<WeeklySchedule, WeeklyScheduleModel, WeeklyScheduleConverter>, IWeeklyScheduleService
    {
        public WeeklyScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        public override WeeklyScheduleModel Add(WeeklyScheduleModel model)
        {
            // convert to Db entity
            var entity = _converter.ToEntity(model);
            // add to database
            entity = Add(entity);
            // add days of week
            model.DaysOfWeek.ForEach(d => entity.WeeklyScheduleDaysOfWeek.Add(new WeeklyScheduleDayOfWeek()
            {
                WeeklyScheduleId = entity.Id,
                DayOfWeek = d
            }));
            // TODO: write days of week in history

            return _converter.ToModel(entity);
        }

        public override WeeklyScheduleModel Update(WeeklyScheduleModel model)
        {
            // get from database
            var dbEntity = GetSingleDb(model);
            // update entity by converter
            dbEntity = _converter.Update(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);
            // clear days of week
            dbEntity.WeeklyScheduleDaysOfWeek.Clear();
            SaveChanges();
            // update days of week
            model.DaysOfWeek.ForEach(d => dbEntity.WeeklyScheduleDaysOfWeek.Add(new WeeklyScheduleDayOfWeek()
            {
                WeeklyScheduleId = dbEntity.Id,
                DayOfWeek = d
            }));
            // TODO: write days of week in history

            return _converter.ToModel(dbEntity);
        }

        protected override IQueryable<WeeklySchedule> Scope => Entities.Include(_ => _.WeeklyScheduleDaysOfWeek);

        protected override int UserId(WeeklySchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }
    }
}
