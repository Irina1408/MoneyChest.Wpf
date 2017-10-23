using MoneyChest.Data.Context;
using MoneyChest.Model.Converters;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public abstract class BaseService<T, TModel, TConverter> : BaseService<T>, IBaseService<TModel>
        where T : class
        where TModel : class
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        protected IEntityModelConverter<T, TModel> _converter;

        public BaseService(ApplicationDbContext context) : base(context)
        {
            _converter = new TConverter();
        }

        public virtual TModel Add(TModel model)
        {
            // convert to Db entity
            var entity = _converter.ToEntity(model);
            // add to database
            entity = Add(entity);
            //SaveChanges();

            return _converter.ToModel(entity);
        }

        public virtual TModel Update(TModel model)
        {
            // get from database
            var dbEntity = GetSingleDb(model);
            // update entity by converter
            dbEntity = _converter.Update(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);

            return _converter.ToModel(dbEntity);
        }

        public virtual void Delete(TModel model) => Delete(GetSingleDb(model));

        protected abstract T GetSingleDb(TModel model);
    }
}
