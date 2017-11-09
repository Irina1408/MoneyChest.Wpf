using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Data.Converters
{
    public class DebtConverter : EntityModelConverterBase<Debt, DebtModel>
    {
        protected override void FillEntity(Debt entity, DebtModel model)
        {
            entity.Description = model.Description;
            entity.DebtType = model.DebtType;
            entity.TakingDate = model.TakingDate;
            entity.Value = model.Value;
            entity.PaidValue = model.PaidValue;
            entity.IsRepaid = model.IsRepaid;
            entity.RepayingDate = model?.RepayingDate;
            entity.Remark = model.Remark;
            entity.CurrencyId = model.CurrencyId;
            entity.CategoryId = model?.CategoryId;
            entity.StorageId = model?.StorageId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Debt entity, DebtModel model)
        {
            model.Id = entity.Id;
            model.Description = entity.Description;
            model.DebtType = entity.DebtType;
            model.TakingDate = entity.TakingDate;
            model.Value = entity.Value;
            model.PaidValue = entity.PaidValue;
            model.IsRepaid = entity.IsRepaid;
            model.RepayingDate = entity?.RepayingDate;
            model.Remark = entity.Remark;
            model.CurrencyId = entity.CurrencyId;
            model.CategoryId = entity?.CategoryId;
            model.StorageId = entity?.StorageId;
            model.UserId = entity.UserId;
            model.Currency = entity.Currency.ToReferenceView();
        }
    }
}
