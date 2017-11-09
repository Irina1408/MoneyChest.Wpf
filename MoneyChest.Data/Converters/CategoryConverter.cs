using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Converters
{
    public class CategoryConverter : EntityModelConverterBase<Category, CategoryModel>
    {
        protected override void FillEntity(Category entity, CategoryModel model)
        {
            entity.Name = model.Name;
            entity.InHistory = model.InHistory;
            entity.TransactionType = model?.TransactionType;
            entity.ParentCategoryId = model?.ParentCategoryId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Category entity, CategoryModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.InHistory = entity.InHistory;
            model.TransactionType = entity?.TransactionType;
            model.ParentCategoryId = entity?.ParentCategoryId;
            model.UserId = entity.UserId;
        }
    }
}
