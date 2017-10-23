using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Converters
{
    public class ForecastSettingConverter : IEntityModelConverter<ForecastSetting, ForecastSettingModel>
    {
        public ForecastSetting ToEntity(ForecastSettingModel model)
        {
            return new ForecastSetting()
            {
                UserId = model.UserId,
                AllCategories = model.AllCategories,
                RepeatsCount = model.RepeatsCount,
                ActualDays = model.ActualDays
            };
        }

        public ForecastSettingModel ToModel(ForecastSetting entity)
        {
            return new ForecastSettingModel()
            {
                UserId = entity.UserId,
                AllCategories = entity.AllCategories,
                RepeatsCount = entity.RepeatsCount,
                ActualDays = entity.ActualDays,
                CategoryIds = entity.Categories.Select(e => e.Id).ToList()
            };
        }

        public ForecastSetting Update(ForecastSetting entity, ForecastSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.AllCategories = model.AllCategories;
            entity.RepeatsCount = model.RepeatsCount;
            entity.ActualDays = model.ActualDays;

            return entity;
        }
    }
}
