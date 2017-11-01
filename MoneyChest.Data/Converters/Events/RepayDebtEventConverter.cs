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
    public class RepayDebtEventConverter : IEntityModelConverter<RepayDebtEvent, RepayDebtEventModel>
    {
        public RepayDebtEvent ToEntity(RepayDebtEventModel model)
        {
            return new RepayDebtEvent()
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
                DebtId = model.DebtId
            };
        }

        public RepayDebtEventModel ToModel(RepayDebtEvent entity)
        {
            return new RepayDebtEventModel()
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
                DebtId = entity.DebtId,
                Storage = entity.Storage.ToReferenceView(),
                Debt = entity.Debt.ToReferenceView(),
                Currency = entity.Debt.Currency.ToReferenceView(),
                DebtCategory = entity.Debt?.Category?.ToReferenceView()
            };
        }

        public RepayDebtEvent Update(RepayDebtEvent entity, RepayDebtEventModel model)
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
            entity.DebtId = model.DebtId;

            return entity;
        }
    }
}
