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
            entity.IncludeWithoutCategory = model.CategoryIds.Count == 0
                || model.CategoryIds.Contains(-1);
            entity.ShowTemplates = model.ShowTemplates;
            entity.IsFilterApplied = model.IsFilterApplied;
            entity.IsFilterVisible = model.IsFilterVisible;
            entity.IsCategoryBranchSelection = model.IsCategoryBranchSelection;
            entity.TransactionType = model.TransactionType;
        }

        protected override void FillModel(DataFilter entity, DataFilterModel model)
        {
            model.Description = entity.Description;
            model.Remark = entity.Remark;
            model.ShowTemplates = entity.ShowTemplates;
            model.IsFilterApplied = entity.IsFilterApplied;
            model.IsFilterVisible = entity.IsFilterVisible;
            model.IsCategoryBranchSelection = entity.IsCategoryBranchSelection;
            model.TransactionType = entity.TransactionType;
            model.CategoryIds = entity.Categories.Select(e => e.Id).ToList();
            model.StorageIds = entity.Storages.Select(e => e.Id).ToList();
            if (entity.IncludeWithoutCategory && model.CategoryIds.Count > 0)
                model.CategoryIds.Add(-1);
        }
    }
}
