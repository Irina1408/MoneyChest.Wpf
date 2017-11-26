using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class CategoryConverter : EntityModelConverterBase<Category, CategoryModel>
    {
        protected override void FillEntity(Category entity, CategoryModel model)
        {
            entity.Name = model.Name;
            entity.IsActive = model.IsActive;
            entity.TransactionType = model?.TransactionType;
            entity.Remark = model.Remark;
            entity.ParentCategoryId = model?.ParentCategoryId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Category entity, CategoryModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.IsActive = entity.IsActive;
            model.TransactionType = entity?.TransactionType;
            model.Remark = entity.Remark;
            model.ParentCategoryId = entity?.ParentCategoryId;
            model.UserId = entity.UserId;
        }
    }
}
