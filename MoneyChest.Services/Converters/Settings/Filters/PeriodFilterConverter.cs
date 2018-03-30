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
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity.DateUntil;
            model.PeriodType = entity.PeriodType;
        }
    }
}
