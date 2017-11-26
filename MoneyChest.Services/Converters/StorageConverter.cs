using MoneyChest.Data.Entities;
using MoneyChest.Data.Extensions;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Converters
{
    public class StorageConverter : EntityModelConverterBase<Storage, StorageModel>
    {
        protected override void FillEntity(Storage entity, StorageModel model)
        {
            entity.Name = model.Name;
            entity.Value = model.Value;
            entity.IsVisible = model.IsVisible;
            entity.Remark = model.Remark;
            entity.StorageGroupId = model.StorageGroupId;
            entity.CurrencyId = model.CurrencyId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Storage entity, StorageModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Value = entity.Value;
            model.IsVisible = entity.IsVisible;
            model.Remark = entity.Remark;
            model.CurrencyId = entity.CurrencyId;
            model.StorageGroupId = entity.StorageGroupId;
            model.UserId = entity.UserId;
            model.Currency = entity.Currency.ToReferenceView();
            model.StorageGroup = entity.StorageGroup.ToReferenceView();
        }
    }
}
