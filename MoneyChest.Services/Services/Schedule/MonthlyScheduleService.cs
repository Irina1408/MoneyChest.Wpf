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
using MoneyChest.Data.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services
{
    public interface IMonthlyScheduleService : IIdManagableServiceBase<MonthlyScheduleModel>
    {
    }

    public class MonthlyScheduleService : HistoricizedIdManageableServiceBase<MonthlySchedule, MonthlyScheduleModel, MonthlyScheduleConverter>, IMonthlyScheduleService
    {
        public MonthlyScheduleService(ApplicationDbContext context) : base(context)
        {
        }

        public override MonthlyScheduleModel Add(MonthlyScheduleModel model)
        {
            // convert to Db entity
            var entity = _converter.ToEntity(model);
            // add to database
            entity = Add(entity);
            // add monthes
            model.Months.ForEach(m => entity.MonthlyScheduleMonths.Add(new MonthlyScheduleMonth()
            {
                MonthlyScheduleId = entity.Id,
                Month = m
            }));
            // TODO: write monthes in history
            
            return _converter.ToModel(entity);
        }

        public override MonthlyScheduleModel Update(MonthlyScheduleModel model)
        {
            // get from database
            var dbEntity = GetSingleDb(model);
            // update entity by converter
            dbEntity = _converter.UpdateEntity(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);
            // clear monthes
            dbEntity.MonthlyScheduleMonths.Clear();
            SaveChanges();
            // update monthes
            model.Months.ForEach(m => dbEntity.MonthlyScheduleMonths.Add(new MonthlyScheduleMonth()
            {
                MonthlyScheduleId = dbEntity.Id,
                Month = m
            }));
            // TODO: write monthes in history

            return _converter.ToModel(dbEntity);
        }

        protected override IQueryable<MonthlySchedule> Scope => Entities.Include(_ => _.MonthlyScheduleMonths);

        protected override int UserId(MonthlySchedule entity)
        {
            if (entity.Event != null) return entity.Event.UserId;
            return _context.Events.FirstOrDefault(_ => _.Id == entity.EventId).UserId;
        }
    }
}
