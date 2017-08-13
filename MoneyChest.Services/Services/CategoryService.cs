using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services
{
    public interface ICategoryService : IBaseHistoricizedService<Category>, IIdManageable<Category>
    {
        int GetCategoryLevelsCount(int userId);
    }

    public class CategoryService : BaseHistoricizedService<Category>, ICategoryService
    {
        #region Initialization

        public CategoryService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region Overrides

        protected override int UserId(Category entity) => entity.UserId;

        protected override Expression<Func<Category, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #endregion

        #region ICategoryService implementation

        public int GetCategoryLevelsCount(int userId)
        {
            int maxLevel = -1;
            var categories = Entities.Where(item => item.ParentCategory == null && item.UserId == userId).ToList();

            foreach (var category in categories)
            {
                int level = CalculateLevelsCount(categories, category);
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

        #endregion

        #region IIdManageable<T> implementation

        public Category Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public List<Category> Get(List<int> ids) => Entities.Where(_ => ids.Contains(_.Id)).ToList();

        public void Delete(int id) => Delete(Get(id));

        #endregion

        #region Private methods

        private int CalculateLevelsCount(List<Category> categories, Category category, int level = 0)
        {
            int maxCatLevel = level;
            foreach (var childCategory in categories.Where(item => item.ParentCategory == category))
            {
                int childCatLevel = CalculateLevelsCount(categories, childCategory, level + 1);
                if (childCatLevel > maxCatLevel)
                    maxCatLevel = childCatLevel;
            }

            return maxCatLevel;
        }

        #endregion
    }
}
