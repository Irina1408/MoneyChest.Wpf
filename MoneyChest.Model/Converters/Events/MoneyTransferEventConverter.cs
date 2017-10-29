using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Model.Convert;

namespace MoneyChest.Model.Converters
{
    public class MoneyTransferEventConverter : IEntityModelConverter<MoneyTransferEvent, MoneyTransferEventModel>
    {
        public MoneyTransferEvent ToEntity(MoneyTransferEventModel model)
        {
            return new MoneyTransferEvent()
            {
                Description = model.Description,
                Value = model.Value,
                EventState = model.EventState,
                PausedToDate = model?.PausedToDate,
                AutoExecution = model.AutoExecution,
                AutoExecutionTime = model?.AutoExecutionTime,
                ConfirmBeforeExecute = model.ConfirmBeforeExecute,
                EventType = model.EventType,
                Remark = model.Remark,
                UserId = model.UserId,
                TakeExistingCurrencyExchangeRate = model.TakeExistingCurrencyExchangeRate,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Commission = model.Commission,
                TakeComissionFromReceiver = model.TakeComissionFromReceiver,
                CommissionType = model.CommissionType,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId,
                CategoryId = model.CategoryId
            };
        }

        public MoneyTransferEventModel ToModel(MoneyTransferEvent entity)
        {
            return new MoneyTransferEventModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                Value = entity.Value,
                EventState = entity.EventState,
                PausedToDate = entity?.PausedToDate,
                AutoExecution = entity.AutoExecution,
                AutoExecutionTime = entity?.AutoExecutionTime,
                ConfirmBeforeExecute = entity.ConfirmBeforeExecute,
                EventType = entity.EventType,
                Remark = entity.Remark,
                UserId = entity.UserId,
                TakeExistingCurrencyExchangeRate = entity.TakeExistingCurrencyExchangeRate,
                CurrencyExchangeRate = entity.CurrencyExchangeRate,
                Commission = entity.Commission,
                TakeComissionFromReceiver = entity.TakeComissionFromReceiver,
                CommissionType = entity.CommissionType,
                StorageFromId = entity.StorageFromId,
                StorageToId = entity.StorageToId,
                StorageFrom = entity.StorageFrom.ToReferenceView(),
                StorageTo = entity.StorageTo.ToReferenceView(),
                StorageFromCurrency = entity.StorageFrom.Currency.ToReferenceView(),
                StorageToCurrency = entity.StorageTo.Currency.ToReferenceView(),
                Category = entity?.Category?.ToReferenceView()
            };
        }

        public MoneyTransferEvent Update(MoneyTransferEvent entity, MoneyTransferEventModel model)
        {
            entity.Id = model.Id;
            entity.Description = model.Description;
            entity.Value = model.Value;
            entity.EventState = model.EventState;
            entity.PausedToDate = model?.PausedToDate;
            entity.AutoExecution = model.AutoExecution;
            entity.AutoExecutionTime = model?.AutoExecutionTime;
            entity.ConfirmBeforeExecute = model.ConfirmBeforeExecute;
            entity.EventType = model.EventType;
            entity.Remark = model.Remark;
            entity.UserId = model.UserId;
            entity.TakeExistingCurrencyExchangeRate = model.TakeExistingCurrencyExchangeRate;
            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Commission = model.Commission;
            entity.TakeComissionFromReceiver = model.TakeComissionFromReceiver;
            entity.CommissionType = model.CommissionType;
            entity.StorageFromId = model.StorageFromId;
            entity.StorageToId = model.StorageToId;
            entity.CategoryId = model?.CategoryId;

            return entity;
        }
    }
}
