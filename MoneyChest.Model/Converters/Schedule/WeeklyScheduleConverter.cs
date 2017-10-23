using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Model.Convert;

namespace MoneyChest.Model.Converters
{
    public class WeeklyScheduleConverter : IEntityModelConverter<WeeklySchedule, WeeklyScheduleModel>
    {
        public WeeklySchedule ToEntity(WeeklyScheduleModel model)
        {
            return new WeeklySchedule()
            {
                ScheduleType = model.ScheduleType,
                EventId = model.EventId,
                DateFrom = model.DateFrom,
                DateUntil = model?.DateUntil,
                Period = model.Period
            };
        }

        public WeeklyScheduleModel ToModel(WeeklySchedule entity)
        {
            return new WeeklyScheduleModel()
            {
                Id = entity.Id,
                ScheduleType = entity.ScheduleType,
                EventId = entity.EventId,
                DateFrom = entity.DateFrom,
                DateUntil = entity?.DateUntil,
                Period = entity.Period,
                DaysOfWeek = entity.WeeklyScheduleDaysOfWeek.Select(e => e.DayOfWeek).ToList()
            };
        }

        public WeeklySchedule Update(WeeklySchedule entity, WeeklyScheduleModel model)
        {
            entity.ScheduleType = model.ScheduleType;
            entity.EventId = model.EventId;
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model?.DateUntil;
            entity.Period = model.Period;

            return entity;
        }
    }
}
