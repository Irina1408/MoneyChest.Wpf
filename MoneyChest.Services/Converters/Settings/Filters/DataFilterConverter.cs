using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class DataFilterConverter : EntityModelConverterBase<DataFilter, DataFilterModel>
    {
        protected override void FillEntity(DataFilter entity, DataFilterModel model)
        {
            entity.Description = model.Description;
            entity.Remark = model.Remark;
            entity.IncludeWithoutCategory = model.IncludeWithoutCategory;
            entity.AllCategories = model.AllCategories;
            entity.IsFilterApplied = model.IsFilterApplied;
            entity.IsFilterVisible = model.IsFilterVisible;
            entity.IsCategoryBranchSelection = model.IsCategoryBranchSelection;
            entity.TransactionType = model.TransactionType;
        }

        protected override void FillModel(DataFilter entity, DataFilterModel model)
        {
            model.Description = entity.Description;
            model.Remark = entity.Remark;
            model.IsFilterApplied = entity.IsFilterApplied;
            model.IsFilterVisible = entity.IsFilterVisible;
            model.IsCategoryBranchSelection = entity.IsCategoryBranchSelection;
            model.TransactionType = entity.TransactionType;
            model.CategoryIds = entity.Categories.Select(e => e.Id).ToList();
            model.StorageIds = entity.Storages.Select(e => e.Id).ToList();
            if (entity.IncludeWithoutCategory && !entity.AllCategories)
                model.CategoryIds.Add(-1);
        }
    }
}
