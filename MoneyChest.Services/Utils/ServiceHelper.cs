using MoneyChest.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Base;

namespace MoneyChest.Services.Utils
{
    internal static class ServiceHelper
    {
        public static void UpdateDescription<T>(ApplicationDbContext context, T entity)
            where T : IHasDescription, IHasCategory
        {
            // check this entity with empty description and populated category
            if (string.IsNullOrEmpty(entity.Description) && entity.CategoryId.HasValue)
            {
                var category = context.Categories.FirstOrDefault(x => x.Id == entity.CategoryId);
                entity.Description = category?.Name;
            }
        }

        public static void UpdateDescription<T>(ApplicationDbContext context, IEnumerable<T> entities)
            where T: IHasDescription, IHasCategory
        {
            // check there is any entity with empty description and populated category
            if (entities.Where(x => string.IsNullOrEmpty(x.Description) && x.CategoryId.HasValue).Any())
            {
                var categoryIds = entities.Where(x => x.CategoryId != null).Select(x => x.CategoryId).Distinct().ToList();
                var categories = context.Categories.Where(x => categoryIds.Contains(x.Id));

                foreach (var entity in entities.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
                {
                    var category = categories.FirstOrDefault(x => x.Id == entity.CategoryId);
                    entity.Description = category?.Name;
                }
            }
        }

        public static bool UpdateRelatedEntities<TRelation>(ApplicationDbContext context, ICollection<TRelation> relatedEntities, List<int> entityIds)
            where TRelation : class, IHasId
        {
            // check parameters
            if (relatedEntities == null || entityIds == null) return false;

            var entitiesToRemove = new List<TRelation>();
            // remove external related entities
            foreach (var entity in relatedEntities.Where(x => !entityIds.Contains(x.Id)))
                entitiesToRemove.Add(entity);
            foreach (var entity in entitiesToRemove)
                relatedEntities.Remove(entity);

            // add new related entities
            var newRelationIds = entityIds.Where(x => !relatedEntities.Any(_ => _.Id == x)).ToList();
            context.Set<TRelation>().Where(_ => newRelationIds.Contains(_.Id)).ToList().ForEach(item => relatedEntities.Add(item));

            return entitiesToRemove.Count > 0 || newRelationIds.Count > 0;
        }
    }
}
