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
    public class RepayDebtEventConverter : EntityModelConverterBase<RepayDebtEvent, RepayDebtEventModel>
    {
        protected override void FillEntity(RepayDebtEvent entity, RepayDebtEventModel model)
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
            entity.DebtId = model.DebtId;
        }

        protected override void FillModel(RepayDebtEvent entity, RepayDebtEventModel model)
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
            model.DebtId = entity.DebtId;
            model.Storage = entity.Storage.ToReferenceView();
            model.Debt = entity.Debt.ToReferenceView();
            model.Currency = entity.Debt.Currency.ToReferenceView();
            model.DebtCategory = entity.Debt?.Category?.ToReferenceView();
        }
    }
}
