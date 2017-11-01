using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Converters
{
    public class CategoryConverter : IEntityModelConverter<Category, CategoryModel>
    {
        public Category ToEntity(CategoryModel model)
        {
            return new Category()
            {
                Name = model.Name,
                InHistory = model.InHistory,
                TransactionType = model?.TransactionType,
                ParentCategoryId = model?.ParentCategoryId,
                UserId = model.UserId
            };
        }

        public CategoryModel ToModel(Category entity)
        {
            return new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                InHistory = entity.InHistory,
                TransactionType = entity?.TransactionType,
                ParentCategoryId = entity?.ParentCategoryId,
                UserId = entity.UserId
            };
        }

        public Category Update(Category entity, CategoryModel model)
        {
            entity.Name = model.Name;
            entity.InHistory = model.InHistory;
            entity.TransactionType = model?.TransactionType;
            entity.ParentCategoryId = model?.ParentCategoryId;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
