﻿using System;
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
        int GetLowestCategoryLevel(int userId);

        /// <summary>
        /// Returns dictionary of category mapping where:
        /// key -> category id,
        /// value -> category id of selected level or higher (1 level is higher then 3 level)
        /// </summary>
        Dictionary<int, int> GetCategoryMapping(int userId, int level);

        /// <summary>
        /// Returns dictionary of category level mapping where:
        /// key -> category id,
        /// value -> category level (from 0)
        /// </summary>
        Dictionary<int, int> GetCategoryLevelMapping(int userId);
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

        public int GetLowestCategoryLevel(int userId)
        {
            int lowestLevel = -1;
            var categories = Entities.Where(item => item.UserId == userId).ToList();
            
            foreach (var category in categories.Where(item => item.ParentCategory == null))
            {
                int level = CalculateLowestLevelNumber(categories, category);
                if (level > lowestLevel)
                    lowestLevel = level;
            }

            return lowestLevel;
        }

        /// <summary>
        /// Returns dictionary of category mapping where:
        /// key -> category id,
        /// value -> category id of selected level or higher (1 level is higher then 3 level)
        /// </summary>
        public Dictionary<int, int> GetCategoryMapping(int userId, int level)
        {
            var categories = GetAllForUser(userId);

            // if level is not declared return the same sequence
            if (level < 0) return categories.ToDictionary(_ => _.Id, _ => _.Id);

            var mapping = GetCategoryLevelMapping(categories);
            var result = new Dictionary<int, int>();

            foreach (var cat in categories)
            {
                if (mapping[cat.Id] <= level)
                    result.Add(cat.Id, cat.Id);
                else
                {
                    // find parent category on necessary level
                    var catId = cat.ParentCategoryId.Value;
                    while (mapping[catId] > level)
                        catId = categories.First(_ => _.Id == catId).ParentCategoryId.Value;
                    // upgrade result
                    result.Add(cat.Id, catId);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns dictionary of category level mapping where:
        /// key -> category id,
        /// value -> category level (from 0)
        /// </summary>
        public Dictionary<int, int> GetCategoryLevelMapping(int userId)
        {
            return GetCategoryLevelMapping(GetAllForUser(userId));
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

        private int CalculateLowestLevelNumber(List<Category> categories, Category category, int level = 0)
        {
            int lowestCatLevel = level;
            foreach (var childCategory in categories.Where(item => item.ParentCategory == category))
            {
                int childCatLevel = CalculateLowestLevelNumber(categories, childCategory, level + 1);
                if (childCatLevel > lowestCatLevel)
                    lowestCatLevel = childCatLevel;
            }

            return lowestCatLevel;
        }

        private Dictionary<int, int> GetCategoryLevelMapping(List<Category> categories)
        {
            int level = 0;
            var result = new Dictionary<int, int>();
            var currentCategories = categories.Where(item => item.ParentCategoryId == null).ToList();

            while (currentCategories.Count > 0)
            {
                // add correspond to current level categories
                foreach (var cat in currentCategories)
                    result.Add(cat.Id, level);

                // update local variables
                currentCategories = categories.Where(item => currentCategories.Any(c => c.Id == item.ParentCategoryId)).ToList();
                level++;
            }

            return result;
        }

        #endregion
    }
}
