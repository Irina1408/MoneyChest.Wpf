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
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Enums;
using MoneyChest.Services.Converters;

namespace MoneyChest.Services.Services
{
    public interface IUserService : IIdManagableServiceBase<UserModel>
    {
        UserModel Get(string name, string password);
        UserModel Get(string name);
    }

    public class UserService : HistoricizedIdManageableServiceBase<User, UserModel, UserConverter>, IUserService
    {
        public UserService(ApplicationDbContext context) : base(context)
        { }

        #region IUserService implementation

        public UserModel Get(string name, string password)
        {
            var user = Scope.FirstOrDefault(_ => _.Name == name && _.Password == password);
            return user != null ? _converter.ToModel(user) : null;
        }

        public UserModel Get(string name)
        {
            var user = Scope.FirstOrDefault(_ => _.Name == name);
            return user != null ? _converter.ToModel(user) : null;
        }

        #endregion

        #region Overrides

        public override void OnAdded(UserModel model, User entity)
        {
            base.OnAdded(model, entity);

            // load defaults
            LoadDefaults(entity, model.Language);
        }

        protected override IQueryable<User> Scope => Entities.Include(_ => _.GeneralSettings);
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
