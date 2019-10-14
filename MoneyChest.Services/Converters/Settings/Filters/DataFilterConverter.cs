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
            model.IsPopulation = true;

            // check every property before update to avoid double changes handling
            if (model.Description != entity.Description) model.Description = entity.Description;
            if (model.Remark != entity.Remark) model.Remark = entity.Remark;
            if (model.IsFilterApplied != entity.IsFilterApplied) model.IsFilterApplied = entity.IsFilterApplied;
            if (model.IsFilterVisible != entity.IsFilterVisible) model.IsFilterVisible = entity.IsFilterVisible;
            if (model.IsCategoryBranchSelection != entity.IsCategoryBranchSelection) model.IsCategoryBranchSelection = entity.IsCategoryBranchSelection;
            if (model.TransactionType != entity.TransactionType) model.TransactionType = entity.TransactionType;

            // build new categories list
            var newCategoriesList = entity.Categories.Select(e => e.Id).ToList();
            if (entity.IncludeWithoutCategory && !entity.AllCategories) newCategoriesList.Add(-1);
            // build new storages list
            var newStoragesList = entity.Storages.Select(e => e.Id).ToList();

            // compare without caring about sequence
            if (!new HashSet<int>(model.CategoryIds).Equals(new HashSet<int>(newCategoriesList))) model.CategoryIds = newCategoriesList;
            if (!new HashSet<int>(model.StorageIds).Equals(new HashSet<int>(newStoragesList))) model.StorageIds = newStoragesList;

            model.IsPopulation = false;
        }
    }
}
