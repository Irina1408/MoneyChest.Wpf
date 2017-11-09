using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Converters
{
    public abstract class EntityModelConverterBase<TEntity, TModel> : IEntityModelConverter<TEntity, TModel>
        where TEntity : new()
        where TModel : new()
    {
        public virtual TEntity ToEntity(TModel model)
        {
            // create new entity
            var entity = new TEntity();

            // update all entity fields
            FillEntity(entity, model);

            return entity;
        }

        public virtual TModel ToModel(TEntity entity)
        {
            // create new model
            var model = new TModel();

            // fill all model fields
            FillModel(entity, model);

            return model;
        }

        public virtual TEntity Update(TEntity entity, TModel model)
        {
            // update all entity fields
            FillEntity(entity, model);

            return entity;
        }

        protected abstract void FillEntity(TEntity entity, TModel model);
        protected abstract void FillModel(TEntity entity, TModel model);
    }
}
