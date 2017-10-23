using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using System.Data.Entity;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Services.Defaults;
using MoneyChest.Model.Model;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services
{
    public interface IUserService : IBaseIdManagableService<UserModel>
    {
        UserModel Add(UserModel model, Language language);
    }

    public class UserService : BaseHistoricizedIdManageableService<User, UserModel, UserConverter>, IUserService
    {
        public UserService(ApplicationDbContext context) : base(context)
        { }

        #region IUserService implementation

        public UserModel Add(UserModel model, Language language)
        {
            User entity = base.Add(_converter.ToEntity(model));
            LoadDefaults(entity, language);
            return _converter.ToModel(entity);
        }

        #endregion

        #region Overrides

        internal override User Add(User entity)
        {
            entity = base.Add(entity);
            LoadDefaults(entity, Language.English);
            return entity;
        }
        protected override int UserId(User entity) => entity.Id;

        #endregion

        #region Private methods

        private void LoadDefaults(User entity, Language language)
        {
            var defaultsLoader = new DefaultsLoadingService(_context);
            defaultsLoader.LoadDefaults(entity.Id, language);
        }

        #endregion
    }
}
