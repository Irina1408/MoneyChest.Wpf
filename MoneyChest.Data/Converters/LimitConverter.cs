using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Data.Converters
{
    public class LimitConverter : EntityModelConverterBase<Limit, LimitModel>
    {
        protected override void FillEntity(Limit entity, LimitModel model)
        {
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model.DateUntil;
            entity.LimitState = model.LimitState;
            entity.Value = model.Value;
            entity.SpentValue = model.SpentValue;
            entity.Remark = model.Remark;
            entity.CurrencyId = model.CurrencyId;
            entity.CategoryId = model?.CategoryId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Limit entity, LimitModel model)
        {
            model.Id = entity.Id;
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity.DateUntil;
            model.LimitState = entity.LimitState;
            model.Value = entity.Value;
            model.SpentValue = entity.SpentValue;
            model.Remark = entity.Remark;
            model.CurrencyId = entity.CurrencyId;
            model.CategoryId = entity?.CategoryId;
            model.UserId = entity.UserId;
            model.Currency = entity.Currency.ToReferenceView();
            model.Category = entity?.Category?.ToReferenceView();
        }
    }
}
