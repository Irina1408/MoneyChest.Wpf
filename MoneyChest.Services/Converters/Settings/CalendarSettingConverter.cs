using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class CalendarSettingConverter : EntityModelConverterBase<CalendarSetting, CalendarSettingModel>
    {
        protected override void FillEntity(CalendarSetting entity, CalendarSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.PeriodType = model.PeriodType;
            entity.ShowLimits = model.ShowLimits;
        }

        protected override void FillModel(CalendarSetting entity, CalendarSettingModel model)
        {
            model.UserId = entity.UserId;
            model.PeriodType = entity.PeriodType;
            model.ShowLimits = entity.ShowLimits;
            model.StorageGroupIds = entity.StorageGroups.Select(e => e.Id).ToList();
        }
    }
}
