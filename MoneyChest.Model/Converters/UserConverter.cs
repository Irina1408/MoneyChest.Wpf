using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Model;

namespace MoneyChest.Model.Converters
{
    public class UserConverter : IEntityModelConverter<User, UserModel>
    {
        public User ToEntity(UserModel model)
        {
            return new User()
            {
                Name = model.Name,
                Password = model.Password,
                LastUsageDate = model.LastUsageDate,
                LastSynchronizationDate = model?.LastSynchronizationDate,
                ServerUserId = model?.ServerUserId
            };
        }

        public UserModel ToModel(User entity)
        {
            return new UserModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Password = entity.Password,
                LastUsageDate = entity.LastUsageDate,
                LastSynchronizationDate = entity?.LastSynchronizationDate,
                ServerUserId = entity?.ServerUserId
            };
        }

        public User Update(User entity, UserModel model)
        {
            entity.Name = model.Name;
            entity.Password = model.Password;
            entity.LastUsageDate = model.LastUsageDate;
            entity.LastSynchronizationDate = model?.LastSynchronizationDate;
            entity.ServerUserId = model?.ServerUserId;

            return entity;
        }
    }
}
