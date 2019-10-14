using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;

namespace MoneyChest.Services.Converters
{
    public class PeriodFilterConverter : EntityModelConverterBase<PeriodFilter, PeriodFilterModel>
    {
        protected override void FillEntity(PeriodFilter entity, PeriodFilterModel model)
        {
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model.DateUntil;
            entity.PeriodType = model.PeriodType;
        }

        protected override void FillModel(PeriodFilter entity, PeriodFilterModel model)
        {
            // check every property before update to avoid double changes handling
            if (model.PeriodType != entity.PeriodType)
            {
                model.PeriodType = entity.PeriodType;

                if (model.PeriodType == Model.Enums.PeriodType.Custom)
                {
                    if (model.DateFrom != entity.DateFrom) model.DateFrom = entity.DateFrom;
                    if (model.DateUntil != entity.DateUntil) model.DateUntil = entity.DateUntil;
                }
            }
        }
    }
}
