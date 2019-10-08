using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Converters
{
    public class LimitConverter : EntityModelConverterBase<Limit, LimitModel>
    {
        protected override void FillEntity(Limit entity, LimitModel model)
        {
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model.DateUntil;
            entity.Description = model.Description;
            entity.Value = model.Value;
            entity.SpentValue = model.SpentValue;
            entity.Remark = model.Remark;
            entity.IncludeWithoutCategory = model.IncludeWithoutCategory;
            entity.AllCategories = model.AllCategories;
            entity.CurrencyId = model.CurrencyId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Limit entity, LimitModel model)
        {
            model.Id = entity.Id;
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity.DateUntil;
            model.Description = entity.Description;
            model.Value = entity.Value;
            model.SpentValue = entity.SpentValue;
            model.Remark = entity.Remark;
            model.CurrencyId = entity.CurrencyId;
            model.UserId = entity.UserId;
            model.Currency = entity.Currency.ToReferenceView();
            model.CategoryIds = entity.Categories.Select(x => x.CategoryId).ToList();
            if (entity.IncludeWithoutCategory && !entity.AllCategories)
                model.CategoryIds.Add(-1);
        }
    }
}
