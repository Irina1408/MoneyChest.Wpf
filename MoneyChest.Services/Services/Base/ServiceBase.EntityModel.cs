using MoneyChest.Data.Context;
using MoneyChest.Data.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public abstract class ServiceBase<T, TModel, TConverter> : ServiceBase<T>, IServiceBase<TModel>
        where T : class
        where TModel : class
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        protected IEntityModelConverter<T, TModel> _converter;

        public ServiceBase(ApplicationDbContext context) : base(context)
        {
            _converter = new TConverter();
        }

        public virtual TModel Add(TModel model)
        {
            // convert to Db entity
            var entity = _converter.ToEntity(model);
            // add to database
            entity = Add(entity);
            // save changes
            SaveChanges();

            return _converter.UpdateModel(GetDbDetailedEntity(entity), model);
        }

        public virtual TModel Update(TModel model)
        {
            // get from database
            var dbEntity = GetDbEntity(model);
            // update entity by converter
            dbEntity = _converter.UpdateEntity(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);
            // save changes
            SaveChanges();

            // TODO: check if related entity foreign key was changed related entity will be updated automatically or not. For now implementation like "not"
            return _converter.UpdateModel(GetDbDetailedEntity(dbEntity), model);
        }

        public virtual List<TModel> Update(IEnumerable<TModel> models)
        {
            // TODO: update list. Not update every model separately 

            //// get from database
            //foreach(var dbEntityMap in GetDbEntitiesMapping(models))
            //{
            //    var model = dbEntityMap.Key;
            //    var dbEntity = dbEntityMap.Value;
            //    // update entity by converter
            //    dbEntity = _converter.UpdateEntity(dbEntity, model);
            //    // update entity in database
            //    dbEntity = Update(dbEntity);
            //}

            //// save changes
            //SaveChanges();

            foreach(var model in models)
                Update(model);

            return models.ToList();
        }

        public virtual void Delete(TModel model)
        {
            Delete(GetDbEntity(model));
            SaveChanges();
        }

        public virtual void Delete(IEnumerable<TModel> models)
        {
            GetDbEntities(models).ForEach(entity => Delete(entity));
            SaveChanges();
        }


        /// <summary>
        /// Returns simple database entity without any references to related entities
        /// </summary>
        protected abstract T GetDbEntity(TModel model);

        /// <summary>
        /// Returns detailed database entity with all necessary for converion to model references to related entities
        /// </summary>
        protected abstract T GetDbDetailedEntity(T entity);

        /// <summary>
        /// Returns list of simple database entities without any references to related entities
        /// </summary>
        protected abstract List<T> GetDbEntities(IEnumerable<TModel> models);

        /// <summary>
        /// Returns dictionary of model and simple database entity without any references to related entities
        /// </summary>
        //protected abstract Dictionary<TModel, T> GetDbEntitiesMapping(IEnumerable<TModel> models);
    }
}
