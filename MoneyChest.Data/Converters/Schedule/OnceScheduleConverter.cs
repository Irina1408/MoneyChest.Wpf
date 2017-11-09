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
    public class OnceScheduleConverter : EntityModelConverterBase<OnceSchedule, OnceScheduleModel>
    {
        protected override void FillEntity(OnceSchedule entity, OnceScheduleModel model)
        {
            entity.ScheduleType = model.ScheduleType;
            entity.EventId = model.EventId;
            entity.Date = model.Date;
        }

        protected override void FillModel(OnceSchedule entity, OnceScheduleModel model)
        {
            model.Id = entity.Id;
            model.ScheduleType = entity.ScheduleType;
            model.EventId = entity.EventId;
            model.Date = entity.Date;
        }
    }
}
