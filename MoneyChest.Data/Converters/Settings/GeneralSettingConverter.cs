﻿using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Converters
{
    public class GeneralSettingConverter : IEntityModelConverter<GeneralSetting, GeneralSettingModel>
    {
        public GeneralSetting ToEntity(GeneralSettingModel model)
        {
            return new GeneralSetting()
            {
                UserId = model.UserId,
                Language = model.Language,
                FirstDayOfWeek = model.FirstDayOfWeek
            };
        }

        public GeneralSettingModel ToModel(GeneralSetting entity)
        {
            return new GeneralSettingModel()
            {
                UserId = entity.UserId,
                Language = entity.Language,
                FirstDayOfWeek = entity.FirstDayOfWeek
            };
        }

        public GeneralSetting Update(GeneralSetting entity, GeneralSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.Language = model.Language;
            entity.FirstDayOfWeek = model.FirstDayOfWeek;

            return entity;
        }
    }
}