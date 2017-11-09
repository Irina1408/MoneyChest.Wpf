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
    public class MonthlyScheduleConverter : EntityModelConverterBase<MonthlySchedule, MonthlyScheduleModel>
    {
        protected override void FillEntity(MonthlySchedule entity, MonthlyScheduleModel model)
        {
            entity.ScheduleType = model.ScheduleType;
            entity.EventId = model.EventId;
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model?.DateUntil;
            entity.DayOfMonth = model.DayOfMonth;
        }

        protected override void FillModel(MonthlySchedule entity, MonthlyScheduleModel model)
        {
            model.Id = entity.Id;
            model.ScheduleType = entity.ScheduleType;
            model.EventId = entity.EventId;
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity?.DateUntil;
            model.DayOfMonth = entity.DayOfMonth;
            model.Months = entity.MonthlyScheduleMonths.Select(e => e.Month).ToList();
        }
    }
}
