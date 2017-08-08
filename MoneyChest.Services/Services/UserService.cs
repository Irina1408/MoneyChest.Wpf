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

namespace MoneyChest.Services.Services
{
    public interface IUserService : IBaseHistoricizedService<User>, IIdManageable<User>
    {
    }

    public class UserService : BaseHistoricizedService<User>, IUserService
    {
        private List<User> _newUsers;

        public UserService(ApplicationDbContext context) : base(context)
        {
            _newUsers = new List<User>();
        }

        public override User Add(User entity)
        {
            _newUsers.Add(entity);
            return Entities.Add(entity);
        }

        public User Add(User entity, Language language)
        {
            _newUsers.Add(entity);
            var user = Entities.Add(entity);
            SaveChanges();

            var defaultsLoader = new DefaultsLoadingService(_context);
            defaultsLoader.LoadDefaults(user.Id, language);

            return user;
        }

        protected override int UserId(User entity) => entity.Id;

        protected override Expression<Func<User, bool>> LimitByUser(int userId) => item => item.Id == userId;

        public override void SaveChanges()
        {
            // save changed items
            SaveChangedItemsToHistory();
            // save changes
            base.SaveChanges();
            // save new users to history
            foreach (var entity in _newUsers)
                _historyService.WriteHistory(entity, ActionType.Add, entity.Id);
            // save history
            _historyService.SaveChanges();
        }

        #region IIdManageable<T> implementation

        public User Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
