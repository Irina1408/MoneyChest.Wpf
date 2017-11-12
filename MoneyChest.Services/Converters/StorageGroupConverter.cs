using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;

namespace MoneyChest.Services.Converters
{
    public class StorageGroupConverter : EntityModelConverterBase<StorageGroup, StorageGroupModel>
    {
        protected override void FillEntity(StorageGroup entity, StorageGroupModel model)
        {
            entity.Name = model.Name;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(StorageGroup entity, StorageGroupModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.UserId = entity.UserId;
        }
    }
}
