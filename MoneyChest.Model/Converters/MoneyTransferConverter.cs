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
                Commission = model.Commission,
                CommissionType = model?.CommissionType,
                TakeComissionFromReceiver = model.TakeComissionFromReceiver,
                TakeComissionCurrencyFromReceiver = model.TakeComissionCurrencyFromReceiver,
                Remark = model.Remark,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId
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
                Commission = entity.Commission,
                CommissionType = entity?.CommissionType,
                TakeComissionFromReceiver = entity.TakeComissionFromReceiver,
                TakeComissionCurrencyFromReceiver = entity.TakeComissionCurrencyFromReceiver,
                Remark = entity.Remark,
                StorageFromId = entity.StorageFromId,
                StorageToId = entity.StorageToId,
                StorageFrom = entity.StorageFrom.ToReferenceView(),
                StorageTo = entity.StorageTo.ToReferenceView()
            };
        }

        public MoneyTransfer Update(MoneyTransfer entity, MoneyTransferModel model)
        {
            entity.Date = model.Date;
            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Value = model.Value;
            entity.Commission = model.Commission;
            entity.CommissionType = model?.CommissionType;
            entity.TakeComissionFromReceiver = model.TakeComissionFromReceiver;
            entity.TakeComissionCurrencyFromReceiver = model.TakeComissionCurrencyFromReceiver;
            entity.Remark = model.Remark;
            entity.StorageFromId = model.StorageFromId;
            entity.StorageToId = model.StorageToId;

            return entity;
        }
    }
}
