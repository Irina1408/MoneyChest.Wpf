using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Base;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services.Base
{
    public interface IUserSettingsService<TModel> : IUserableService<TModel>
    {
        TModel Update(TModel model);
    }

    public abstract class BaseUserSettingService<T, TModel, TConverter> : BaseService<T>, IUserSettingsService<TModel>
        where T : class, IHasUserId
        where TModel : class, IHasUserId
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        protected IEntityModelConverter<T, TModel> _converter;

        public BaseUserSettingService(ApplicationDbContext context) : base(context)
        {
            _converter = new TConverter();
        }

        public virtual TModel GetForUser(int userId) => _converter.ToModel(Scope.FirstOrDefault(e => e.UserId == userId));

        public virtual TModel Update(TModel model)
        {
            // get from database
            var dbEntity = Scope.FirstOrDefault(e => e.UserId == model.UserId);
            // update entity by converter (update fields)
            dbEntity = _converter.Update(dbEntity, model);
            // update related entities
            dbEntity = Update(dbEntity, model);
            // update entity in database
            dbEntity = Update(dbEntity);
            // save changes
            SaveChanges();

            return _converter.ToModel(dbEntity);
        }

        /// <summary>
        /// Should update related entities (do nothing by default)
        /// </summary>
        protected virtual T Update(T entity, TModel model) => entity;
    }
}
