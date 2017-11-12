using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class GeneralSettingConverter : EntityModelConverterBase<GeneralSetting, GeneralSettingModel>
    {
        protected override void FillEntity(GeneralSetting entity, GeneralSettingModel model)
        {
            entity.UserId = model.UserId;
            entity.Language = model.Language;
            entity.FirstDayOfWeek = model.FirstDayOfWeek;
        }

        protected override void FillModel(GeneralSetting entity, GeneralSettingModel model)
        {
            model.UserId = entity.UserId;
            model.Language = entity.Language;
            model.FirstDayOfWeek = entity.FirstDayOfWeek;
        }
    }
}
