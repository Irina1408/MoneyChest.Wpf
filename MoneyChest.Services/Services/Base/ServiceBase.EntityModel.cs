using MoneyChest.Data.Context;
using MoneyChest.Services.Converters;
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
        #region Protected fields

        // TODO: make private and use method Convert
        protected IEntityModelConverter<T, TModel> _converter;

        #endregion

        #region Initialization

        public ServiceBase(ApplicationDbContext context) : base(context)
        {
            _converter = new TConverter();
        }

        #endregion

        #region Create/Add/Update/Delete methods
        
        public virtual TModel PrepareNew(TModel model) => model;

        public virtual TModel Add(TModel model)
        {
            // convert to Db entity
            var entity = _converter.ToEntity(model);
            // add to database
            entity = Add(entity);
            // save changes
            SaveChanges();
            // call OnAdded method
            OnAdded(model, entity);

            return _converter.UpdateModel(GetDbDetailedEntity(entity), model);
        }

        public virtual IEnumerable<TModel> Add(IEnumerable<TModel> models)
        {
            // convert to Db entities
            var entities = models.Select(x => new { Entity = _converter.ToEntity(x), Model = x }).ToList();
            // add to database
            Add(entities.Select(x => x.Entity).AsEnumerable());
            // save changes
            SaveChanges();
            // call OnAdded method
            foreach(var entity in entities)
                OnAdded(entity.Model, entity.Entity);

            return entities.Select(x => _converter.UpdateModel(GetDbDetailedEntity(x.Entity), x.Model)).AsEnumerable();
        }

        public virtual TModel Update(TModel model)
        {
            // get from database
            var dbEntity = GetDbEntity(model);
            // get old model
            var oldModel = _converter.ToModel(dbEntity);
            // update entity by converter
            dbEntity = _converter.UpdateEntity(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);
            // save changes
            SaveChanges();
            // call OnUpdated method
            OnUpdated(oldModel, model);

            // TODO: check if related entity foreign key was changed related entity will be updated automatically or not. For now implementation like "not"
            return _converter.UpdateModel(GetDbDetailedEntity(dbEntity), model);
        }

        public virtual IEnumerable<TModel> Update(IEnumerable<TModel> models)
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

            return models.AsEnumerable();
        }

        public virtual void Delete(TModel model)
        {
            Delete(GetDbEntity(model));
            SaveChanges();
            // call OnDeleted method
            OnDeleted(model);
        }

        public virtual void Delete(IEnumerable<TModel> models)
        {
            // TODO: remove all range by service.Delete(IEnumerable<T> models)
            GetDbEntities(models).ForEach(entity => Delete(entity));
            SaveChanges();
            // call OnDeleted method
            foreach(var model in models)
                OnDeleted(model);
        }

        public virtual void OnAdded(TModel model, T entity) { }
        public virtual void OnUpdated(TModel oldModel, TModel model) { }
        public virtual void OnDeleted(TModel model) { }

        #endregion

        #region Internal & protected methods

        internal protected TModel Convert(T entity) => _converter.ToModel(entity);
        internal protected T Convert(TModel model) => _converter.ToEntity(model);
        internal protected List<TModel> Convert(List<T> entities) => entities.ConvertAll(Convert);

        #endregion

        #region Methods to override

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

        #endregion
    }
}
