using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using MoneyChest.Data.Extensions;
using MoneyChest.Utils.SerializationUtils;

namespace MoneyChest.Services.Converters
{
    public class MoneyTransferEventConverter : EntityModelConverterBase<MoneyTransferEvent, MoneyTransferEventModel>
    {
        protected override void FillEntity(MoneyTransferEvent entity, MoneyTransferEventModel model)
        {
            entity.Description = model.Description;
            entity.Value = model.Value;
            entity.EventState = model.EventState;
            entity.Schedule = SerializationUtils.Serialize(model.Schedule);
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model?.DateUntil;
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
            entity.TakeCommissionFromReceiver = model.TakeCommissionFromReceiver;
            entity.CommissionType = model.CommissionType;
            entity.StorageFromId = model.StorageFromId;
            entity.StorageToId = model.StorageToId;
            entity.CategoryId = model?.CategoryId;
        }

        protected override void FillModel(MoneyTransferEvent entity, MoneyTransferEventModel model)
        {
            model.Id = entity.Id;
            model.Description = entity.Description;
            model.Value = entity.Value;
            model.EventState = entity.EventState;
            model.Schedule = SerializationUtils.Deserialize<ScheduleModel>(entity.Schedule) ?? new ScheduleModel(true);
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity?.DateUntil;
            model.PausedToDate = entity?.PausedToDate;
            model.AutoExecution = entity.AutoExecution;
            model.AutoExecutionTime = entity?.AutoExecutionTime;
            model.ConfirmBeforeExecute = entity.ConfirmBeforeExecute;
            model.EventType = entity.EventType;
            model.Remark = entity.Remark;
            model.UserId = entity.UserId;

            model.TakeExistingCurrencyExchangeRate = entity.TakeExistingCurrencyExchangeRate;
            model.CurrencyExchangeRate = entity.CurrencyExchangeRate;
            model.Commission = entity.Commission;
            model.TakeCommissionFromReceiver = entity.TakeCommissionFromReceiver;
            model.CommissionType = entity.CommissionType;
            model.StorageFromId = entity.StorageFromId;
            model.StorageToId = entity.StorageToId;
            model.StorageFrom = entity.StorageFrom.ToReferenceView();
            model.StorageTo = entity.StorageTo.ToReferenceView();
            model.StorageFromCurrency = entity.StorageFrom.Currency.ToReferenceView();
            model.StorageToCurrency = entity.StorageTo.Currency.ToReferenceView();
            model.Category = entity?.Category?.ToReferenceView();
        }
    }
}
