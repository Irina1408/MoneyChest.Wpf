using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Converters
{
    public class RecordsViewFilterConverter : EntityModelConverterBase<RecordsViewFilter, RecordsViewFilterModel>
    {
        protected override void FillEntity(RecordsViewFilter entity, RecordsViewFilterModel model)
        {
            entity.UserId = model.UserId;
            entity.AllCategories = model.AllCategories;
            entity.Description = model.Description;
            entity.Remark = model.Remark;
            entity.PeriodFilterType = model.PeriodFilterType;
            entity.TransactionType = model.TransactionType;
            entity.DateFrom = model?.DateFrom;
            entity.DateUntil = model?.DateUntil;
        }

        protected override void FillModel(RecordsViewFilter entity, RecordsViewFilterModel model)
        {
            model.UserId = entity.UserId;
            model.AllCategories = entity.AllCategories;
            model.Description = entity.Description;
            model.Remark = entity.Remark;
            model.PeriodFilterType = entity.PeriodFilterType;
            model.TransactionType = entity.TransactionType;
            model.DateFrom = entity?.DateFrom;
            model.DateUntil = entity?.DateUntil;
            model.CategoryIds = entity.Categories.Select(e => e.Id).ToList();
        }
    }
}
