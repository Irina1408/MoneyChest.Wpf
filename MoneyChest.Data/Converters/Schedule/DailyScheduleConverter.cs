using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;

namespace MoneyChest.Data.Converters
{
    public class DailyScheduleConverter : IEntityModelConverter<DailySchedule, DailyScheduleModel>
    {
        public DailySchedule ToEntity(DailyScheduleModel model)
        {
            return new DailySchedule()
            {
                ScheduleType = model.ScheduleType,
                EventId = model.EventId,
                DateFrom = model.DateFrom,
                DateUntil = model?.DateUntil,
                Period = model.Period
            };
        }

        public DailyScheduleModel ToModel(DailySchedule entity)
        {
            return new DailyScheduleModel()
            {
                Id = entity.Id,
                ScheduleType = entity.ScheduleType,
                EventId = entity.EventId,
                DateFrom = entity.DateFrom,
                DateUntil = entity?.DateUntil,
                Period = entity.Period
            };
        }

        public DailySchedule Update(DailySchedule entity, DailyScheduleModel model)
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
