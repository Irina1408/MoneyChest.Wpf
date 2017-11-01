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
    public class MonthlyScheduleConverter : IEntityModelConverter<MonthlySchedule, MonthlyScheduleModel>
    {
        public MonthlySchedule ToEntity(MonthlyScheduleModel model)
        {
            return new MonthlySchedule()
            {
                ScheduleType = model.ScheduleType,
                EventId = model.EventId,
                DateFrom = model.DateFrom,
                DateUntil = model?.DateUntil,
                DayOfMonth = model.DayOfMonth
            };
        }

        public MonthlyScheduleModel ToModel(MonthlySchedule entity)
        {
            return new MonthlyScheduleModel()
            {
                Id = entity.Id,
                ScheduleType = entity.ScheduleType,
                EventId = entity.EventId,
                DateFrom = entity.DateFrom,
                DateUntil = entity?.DateUntil,
                DayOfMonth = entity.DayOfMonth,
                Months = entity.MonthlyScheduleMonths.Select(e => e.Month).ToList()
            };
        }

        public MonthlySchedule Update(MonthlySchedule entity, MonthlyScheduleModel model)
        {
            entity.ScheduleType = model.ScheduleType;
            entity.EventId = model.EventId;
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model?.DateUntil;
            entity.DayOfMonth = model.DayOfMonth;

            return entity;
        }
    }
}
