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
    public class OnceScheduleConverter : IEntityModelConverter<OnceSchedule, OnceScheduleModel>
    {
        public OnceSchedule ToEntity(OnceScheduleModel model)
        {
            return new OnceSchedule()
            {
                ScheduleType = model.ScheduleType,
                EventId = model.EventId,
                Date = model.Date
            };
        }

        public OnceScheduleModel ToModel(OnceSchedule entity)
        {
            return new OnceScheduleModel()
            {
                Id = entity.Id,
                ScheduleType = entity.ScheduleType,
                EventId = entity.EventId,
                Date = entity.Date
            };
        }

        public OnceSchedule Update(OnceSchedule entity, OnceScheduleModel model)
        {
            entity.ScheduleType = model.ScheduleType;
            entity.EventId = model.EventId;
            entity.Date = model.Date;

            return entity;
        }
    }
}
