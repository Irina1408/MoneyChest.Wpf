using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Converters
{
    public class CalendarSettingConverter : IEntityModelConverter<CalendarSetting, CalendarSettingModel>
    {
        public CalendarSetting ToEntity(CalendarSettingModel model)
        {
            return new CalendarSetting()
            {
                UserId = model.UserId,
                PeriodType = model.PeriodType,
                ShowLimits = model.ShowLimits
            };
        }

        public CalendarSettingModel ToModel(CalendarSetting entity)
        {
            return new CalendarSettingModel()
            {
                UserId = entity.UserId,
                PeriodType = entity.PeriodType,
                ShowLimits = entity.ShowLimits,
                StorageGroupIds = entity.StorageGroups.Select(e => e.Id).ToList()
            };
        }

        public CalendarSetting Update(CalendarSetting entity, CalendarSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.PeriodType = model.PeriodType;
            entity.ShowLimits = model.ShowLimits;

            return entity;
        }
    }
}
