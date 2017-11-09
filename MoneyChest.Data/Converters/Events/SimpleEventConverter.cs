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
    public class SimpleEventConverter : EntityModelConverterBase<SimpleEvent, SimpleEventModel>
    {
        protected override void FillEntity(SimpleEvent entity, SimpleEventModel model)
        {
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
        }

        protected override void FillModel(SimpleEvent entity, SimpleEventModel model)
        {
            model.Id = entity.Id;
            model.Description = entity.Description;
            model.Value = entity.Value;
            model.EventState = entity.EventState;
            model.PausedToDate = entity?.PausedToDate;
            model.AutoExecution = entity.AutoExecution;
            model.AutoExecutionTime = entity?.AutoExecutionTime;
            model.ConfirmBeforeExecute = entity.ConfirmBeforeExecute;
            model.EventType = entity.EventType;
            model.Remark = entity.Remark;
            model.UserId = entity.UserId;
            model.StorageId = entity.StorageId;
            model.TransactionType = entity.TransactionType;
            model.CategoryId = entity.CategoryId;
            model.CurrencyId = entity.CurrencyId;
            model.Storage = entity.Storage.ToReferenceView();
            model.Currency = entity.Currency.ToReferenceView();
            model.Category = entity?.Category.ToReferenceView();
        }
    }
}
