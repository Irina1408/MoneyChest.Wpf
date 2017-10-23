using MoneyChest.Data.Entities;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Model.Converters
{
    public class StorageConverter : IEntityModelConverter<Storage, StorageModel>
    {
        public Storage ToEntity(StorageModel model)
        {
            return new Storage()
            {
                Name = model.Name,
                Value = model.Value,
                Remark = model.Remark,
                CurrencyId = model.CurrencyId,
                StorageGroupId = model.StorageGroupId,
                UserId = model.UserId
            };
        }

        public StorageModel ToModel(Storage entity)
        {
            return new StorageModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Value = entity.Value,
                Remark = entity.Remark,
                CurrencyId = entity.CurrencyId,
                StorageGroupId = entity.StorageGroupId,
                UserId = entity.UserId,
                Currency = entity.Currency.ToReferenceView(),
                StorageGroup = entity.StorageGroup.ToReferenceView()
            };
        }

        public Storage Update(Storage entity, StorageModel model)
        {
            entity.Name = model.Name;
            entity.Value = model.Value;
            entity.Remark = model.Remark;
            entity.StorageGroupId = model.StorageGroupId;
            entity.CurrencyId = model.CurrencyId;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
