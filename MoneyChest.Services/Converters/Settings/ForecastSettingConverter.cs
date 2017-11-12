using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class ForecastSettingConverter : EntityModelConverterBase<ForecastSetting, ForecastSettingModel>
    {
        protected override void FillEntity(ForecastSetting entity, ForecastSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.AllCategories = model.AllCategories;
            entity.RepeatsCount = model.RepeatsCount;
            entity.ActualDays = model.ActualDays;
        }

        protected override void FillModel(ForecastSetting entity, ForecastSettingModel model)
        {
            model.UserId = entity.UserId;
            model.AllCategories = entity.AllCategories;
            model.RepeatsCount = entity.RepeatsCount;
            model.ActualDays = entity.ActualDays;
            model.CategoryIds = entity.Categories.Select(e => e.Id).ToList();
        }
    }
}
