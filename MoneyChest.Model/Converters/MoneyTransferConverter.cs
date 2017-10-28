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
    public class MoneyTransferConverter : IEntityModelConverter<MoneyTransfer, MoneyTransferModel>
    {
        public MoneyTransfer ToEntity(MoneyTransferModel model)
        {
            return new MoneyTransfer()
            {
                Date = model.Date,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Value = model.Value,
                Description = model.Description,
                Commission = model.Commission,
                CommissionType = model?.CommissionType,
                TakeComissionFromReceiver = model.TakeComissionFromReceiver,
                Remark = model.Remark,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId,
                CategoryId = model.CategoryId
            };
        }

        public MoneyTransferModel ToModel(MoneyTransfer entity)
        {
            return new MoneyTransferModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                CurrencyExchangeRate = entity.CurrencyExchangeRate,
                Value = entity.Value,
                Description = entity.Description,
                Commission = entity.Commission,
                CommissionType = entity?.CommissionType,
                TakeComissionFromReceiver = entity.TakeComissionFromReceiver,
                Remark = entity.Remark,
                StorageFromId = entity.StorageFromId,
                StorageToId = entity.StorageToId,
                CategoryId = entity.CategoryId,
                StorageFrom = entity.StorageFrom.ToReferenceView(),
                StorageTo = entity.StorageTo.ToReferenceView(),
                Category = entity?.Category.ToReferenceView()
            };
        }

        public MoneyTransfer Update(MoneyTransfer entity, MoneyTransferModel model)
        {
            entity.Date = model.Date;
            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Value = model.Value;
            entity.Description = model.Description;
            entity.Commission = model.Commission;
            entity.CommissionType = model?.CommissionType;
            entity.TakeComissionFromReceiver = model.TakeComissionFromReceiver;
            entity.Remark = model.Remark;
            entity.StorageFromId = model.StorageFromId;
            entity.StorageToId = model.StorageToId;
            entity.CategoryId = model.CategoryId;

            return entity;
        }
    }
}
