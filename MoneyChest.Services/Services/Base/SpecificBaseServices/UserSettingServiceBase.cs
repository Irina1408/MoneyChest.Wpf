using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Model.Base;
using MoneyChest.Services.Converters;

namespace MoneyChest.Services.Services.Base
{
    public interface IUserSettingsService<TModel> : IUserableService<TModel>
    {
        TModel Update(TModel model);
    }

    public abstract class UserSettingServiceBase<T, TModel, TConverter> : ServiceBase<T>, IUserSettingsService<TModel>
        where T : class, IHasUserId, new()
        where TModel : class, IHasUserId
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        protected IEntityModelConverter<T, TModel> _converter;

        public UserSettingServiceBase(ApplicationDbContext context) : base(context)
        {
            _converter = new TConverter();
        }

        public virtual TModel GetForUser(int userId)
        {
            var settings = Scope.FirstOrDefault(e => e.UserId == userId);
            if (settings == null)
            {
                Entities.Add(new T() { UserId = userId });
                _context.SaveChanges();

                settings = Scope.FirstOrDefault(e => e.UserId == userId);
            }

            return _converter.ToModel(settings);
        }

        public virtual TModel Update(TModel model)
        {
            // get from database
            var dbEntity = Scope.FirstOrDefault(e => e.UserId == model.UserId);
            // update entity by converter (update fields)
            dbEntity = _converter.UpdateEntity(dbEntity, model);
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
