using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Data.Converters
{
    public class SimpleEventConverter : IEntityModelConverter<SimpleEvent, SimpleEventModel>
    {
        public SimpleEvent ToEntity(SimpleEventModel model)
        {
            return new SimpleEvent()
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
                StorageId = model.StorageId,
                TransactionType = model.TransactionType,
                CategoryId = model.CategoryId,
                CurrencyId = model.CurrencyId
            };
        }

        public SimpleEventModel ToModel(SimpleEvent entity)
        {
            return new SimpleEventModel()
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
                StorageId = entity.StorageId,
                TransactionType = entity.TransactionType,
                CategoryId = entity.CategoryId,
                CurrencyId = entity.CurrencyId,
                Storage = entity.Storage.ToReferenceView(),
                Currency = entity.Currency.ToReferenceView(),
                Category = entity?.Category.ToReferenceView()
            };
        }

        public SimpleEvent Update(SimpleEvent entity, SimpleEventModel model)
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
            entity.StorageId = model.StorageId;
            entity.TransactionType = model.TransactionType;
            entity.CurrencyId = model.CurrencyId;
            entity.CategoryId = model.CategoryId;

            return entity;
        }
    }
}
