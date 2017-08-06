using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public interface ICategoryService : IBaseHistoricizedService<Category>
    {
        int GetCategoryLevelsCount(int userId);
    }

    public class CategoryService : BaseHistoricizedService<Category>, ICategoryService
    {
        public CategoryService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Category entity) => entity.UserId;

        protected override Func<Category, bool> LimitByUser(int userId) => item => item.UserId == userId;

        public int GetCategoryLevelsCount(int userId)
        {
            int maxLevel = -1;
            var categories = Entities.Where(item => item.ParentCategory == null && item.UserId == userId).ToList();

            foreach (var category in categories)
            {
                int level = CalculateLevelsCount(category);
                if (level > maxLevel)
                    maxLevel = level;
            }

            return maxLevel;
        }

        public void ReplaceRelatedEntities(int categoryIdFrom, int categoryIdTo)
        {
            // replace records
            _context.Records
                .Where(item => item.CategoryId == categoryIdFrom)
                .ToList()
                .ForEach(item => item.CategoryId = categoryIdTo);

            // replace limits
            _context.Limits
                .Where(item => item.CategoryId == categoryIdFrom)
                .ToList()
                .ForEach(item => item.CategoryId = categoryIdTo);

            // replace events
            _context.SimpleEvents
                .Where(item => item.CategoryId == categoryIdFrom)
                .ToList()
                .ForEach(item => item.CategoryId = categoryIdTo);
        }

        #region Private methods

        private int CalculateLevelsCount(Category category, int level = 0)
        {
            int maxCatLevel = level;
            foreach (var childCategory in Entities.Where(item => item.ParentCategory == category))
            {
                int childCatLevel = CalculateLevelsCount(childCategory, level + 1);
                if (childCatLevel > maxCatLevel)
                    maxCatLevel = childCatLevel;
            }

            return maxCatLevel;
        }

        #endregion
    }
}
