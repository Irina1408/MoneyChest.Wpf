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
    public class StorageGroupConverter : IEntityModelConverter<StorageGroup, StorageGroupModel>
    {
        public StorageGroup ToEntity(StorageGroupModel model)
        {
            return new StorageGroup()
            {
                Name = model.Name,
                UserId = model.UserId
            };
        }

        public StorageGroupModel ToModel(StorageGroup entity)
        {
            return new StorageGroupModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                UserId = entity.UserId
            };
        }

        public StorageGroup Update(StorageGroup entity, StorageGroupModel model)
        {
            entity.Name = model.Name;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
