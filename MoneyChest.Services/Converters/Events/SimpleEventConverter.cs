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
    public class SimpleEventConverter : EntityModelConverterBase<SimpleEvent, SimpleEventModel>
    {
        protected override void FillEntity(SimpleEvent entity, SimpleEventModel model)
        {
            entity.Description = model.Description;
            entity.Value = model.Value;
            entity.EventState = model.EventState;
            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Commission = model.Commission;
            entity.CommissionType = model.CommissionType;
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

            entity.StorageId = model.StorageId;
            entity.RecordType = model.RecordType;
            entity.CurrencyId = model.CurrencyId;
            entity.CategoryId = model.CategoryId;
        }

        protected override void FillModel(SimpleEvent entity, SimpleEventModel model)
        {
            model.Id = entity.Id;
            model.Description = entity.Description;
            model.Value = entity.Value;
            model.EventState = entity.EventState;
            model.CurrencyExchangeRate = entity.CurrencyExchangeRate;
            model.Commission = entity.Commission;
            model.CommissionType = entity.CommissionType;
            model.Schedule = SerializationUtils.Deserialize<ScheduleModel>(entity.Schedule) ?? new ScheduleModel();
            model.DateFrom = entity.DateFrom;
            model.DateUntil = entity?.DateUntil;
            model.PausedToDate = entity?.PausedToDate;
            model.AutoExecution = entity.AutoExecution;
            model.AutoExecutionTime = entity?.AutoExecutionTime;
            model.ConfirmBeforeExecute = entity.ConfirmBeforeExecute;
            model.EventType = entity.EventType;
            model.Remark = entity.Remark;
            model.UserId = entity.UserId;

            model.StorageId = entity.StorageId;
            model.RecordType = entity.RecordType;
            model.CategoryId = entity.CategoryId;
            model.CurrencyId = entity.CurrencyId;
            model.Storage = entity.Storage.ToReferenceView();
            model.Currency = entity.Currency.ToReferenceView();
            model.Category = entity.Category?.ToReferenceView();
        }
    }
}
