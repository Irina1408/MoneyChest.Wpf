using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Converters
{
    public class GeneralSettingConverter : IEntityModelConverter<GeneralSetting, GeneralSettingModel>
    {
        public GeneralSetting ToEntity(GeneralSettingModel model)
        {
            return new GeneralSetting()
            {
                UserId = model.UserId,
                HideCoinBoxStorages = model.HideCoinBoxStorages,
                Language = model.Language,
                FirstDayOfWeek = model.FirstDayOfWeek,
                DebtCategoryId = model.DebtCategoryId,
                ComissionCategoryId = model.ComissionCategoryId
            };
        }

        public GeneralSettingModel ToModel(GeneralSetting entity)
        {
            return new GeneralSettingModel()
            {
                UserId = entity.UserId,
                HideCoinBoxStorages = entity.HideCoinBoxStorages,
                Language = entity.Language,
                FirstDayOfWeek = entity.FirstDayOfWeek,
                DebtCategoryId = entity.DebtCategoryId,
                ComissionCategoryId = entity.ComissionCategoryId
            };
        }

        public GeneralSetting Update(GeneralSetting entity, GeneralSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.HideCoinBoxStorages = model.HideCoinBoxStorages;
            entity.Language = model.Language;
            entity.FirstDayOfWeek = entity.FirstDayOfWeek;
            entity.DebtCategoryId = entity.DebtCategoryId;
            entity.ComissionCategoryId = entity.ComissionCategoryId;

            return entity;
        }
    }
}
