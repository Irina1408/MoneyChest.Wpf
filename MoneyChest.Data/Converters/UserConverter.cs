using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;

namespace MoneyChest.Data.Converters
{
    public class UserConverter : EntityModelConverterBase<User, UserModel>
    {
        protected override void FillEntity(User entity, UserModel model)
        {
            entity.Name = model.Name;
            entity.Password = model.Password;
            entity.LastUsageDate = model.LastUsageDate;
            entity.LastSynchronizationDate = model?.LastSynchronizationDate;
            entity.ServerUserId = model?.ServerUserId;
        }

        protected override void FillModel(User entity, UserModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Password = entity.Password;
            model.LastUsageDate = entity.LastUsageDate;
            model.LastSynchronizationDate = entity?.LastSynchronizationDate;
            model.ServerUserId = entity?.ServerUserId;
        }
    }
}
