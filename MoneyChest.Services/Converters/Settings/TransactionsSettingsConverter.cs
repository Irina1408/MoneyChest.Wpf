using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class TransactionsSettingsConverter : EntityModelConverterBase<TransactionsSettings, TransactionsSettingsModel>
    {
        protected override void FillEntity(TransactionsSettings entity, TransactionsSettingsModel model)
        {
            entity.UserId = model.UserId;
            //entity.AllCategories = model.AllCategories;
            //entity.Description = model.Description;
            //entity.Remark = model.Remark;
            //entity.PeriodFilterType = model.PeriodFilterType;
            //entity.RecordType = model.RecordType;
            //entity.DateFrom = model?.DateFrom;
            //entity.DateUntil = model?.DateUntil;
        }

        protected override void FillModel(TransactionsSettings entity, TransactionsSettingsModel model)
        {
            model.UserId = entity.UserId;
            //model.AllCategories = entity.AllCategories;
            //model.Description = entity.Description;
            //model.Remark = entity.Remark;
            //model.PeriodFilterType = entity.PeriodFilterType;
            //model.RecordType = entity.RecordType;
            //model.DateFrom = entity?.DateFrom;
            //model.DateUntil = entity?.DateUntil;
            //model.CategoryIds = entity.Categories.Select(e => e.Id).ToList();
        }
    }
}
