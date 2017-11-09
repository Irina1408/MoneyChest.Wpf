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
    public class DailyScheduleConverter : EntityModelConverterBase<DailySchedule, DailyScheduleModel>
    {
        protected override void FillEntity(DailySchedule entity, DailyScheduleModel model)
        {
            entity.ScheduleType = model.ScheduleType;
            entity.EventId = model.EventId;
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model?.DateUntil;
            entity.Period = model.Period;
        }

        protected override void FillModel(DailySchedule entity, DailyScheduleModel model)
        {
            model.Id = entity.Id;
            model.ScheduleType = entity.ScheduleType;
            model.EventId = entity.EventId;
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity?.DateUntil;
            model.Period = entity.Period;
        }
    }
}
