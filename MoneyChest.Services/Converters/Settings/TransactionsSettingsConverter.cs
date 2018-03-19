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

            if(entity.PeriodFilter != null)
            {
                entity.PeriodFilter.DateFrom = model.PeriodFilter.DateFrom;
                entity.PeriodFilter.DateUntil = model.PeriodFilter.DateUntil;
                entity.PeriodFilter.PeriodType = model.PeriodFilter.PeriodType;
            }

            if(entity.DataFilter != null)
            {
                entity.DataFilter.Description = model.DataFilter.Description;
                entity.DataFilter.Remark = model.DataFilter.Remark;
                entity.DataFilter.IncludeWithoutCategory = model.DataFilter.IncludeWithoutCategory;
                entity.DataFilter.IsFilterApplied = model.DataFilter.IsFilterApplied;
                entity.DataFilter.IsSingleCategorySelection = model.DataFilter.IsSingleCategorySelection;
                entity.DataFilter.TransactionType = model.DataFilter.TransactionType;
            }
        }

        protected override void FillModel(TransactionsSettings entity, TransactionsSettingsModel model)
        {
            model.UserId = entity.UserId;
            if (entity.PeriodFilter != null)
            {
                model.PeriodFilter.DateFrom = entity.PeriodFilter.DateFrom;
                model.PeriodFilter.DateUntil = entity.PeriodFilter.DateUntil;
                model.PeriodFilter.PeriodType = entity.PeriodFilter.PeriodType;
            }

            if (entity.DataFilter != null)
            {
                model.DataFilter.Description = entity.DataFilter.Description;
                model.DataFilter.Remark = entity.DataFilter.Remark;
                model.DataFilter.IncludeWithoutCategory = entity.DataFilter.IncludeWithoutCategory;
                model.DataFilter.IsFilterApplied = entity.DataFilter.IsFilterApplied;
                model.DataFilter.IsSingleCategorySelection = entity.DataFilter.IsSingleCategorySelection;
                model.DataFilter.TransactionType = entity.DataFilter.TransactionType;
                model.DataFilter.CategoryIds = entity.DataFilter.Categories.Select(e => e.Id).ToList();
                model.DataFilter.StorageIds = entity.DataFilter.Storages.Select(e => e.Id).ToList();
            }
        }
    }
}
