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
    public class DebtConverter : IEntityModelConverter<Debt, DebtModel>
    {
        public Debt ToEntity(DebtModel model)
        {
            return new Debt()
            {
                Description = model.Description,
                DebtType = model.DebtType,
                TakingDate = model.TakingDate,
                Value = model.Value,
                PaidValue = model.PaidValue,
                IsRepaid = model.IsRepaid,
                RepayingDate = model?.RepayingDate,
                Remark = model.Remark,
                CurrencyId = model.CurrencyId,
                CategoryId = model?.CategoryId,
                StorageId = model?.StorageId,
                UserId = model.UserId
            };
        }

        public DebtModel ToModel(Debt entity)
        {
            return new DebtModel()
            {
                Id = entity.Id,
                Description = entity.Description,
                DebtType = entity.DebtType,
                TakingDate = entity.TakingDate,
                Value = entity.Value,
                PaidValue = entity.PaidValue,
                IsRepaid = entity.IsRepaid,
                RepayingDate = entity?.RepayingDate,
                Remark = entity.Remark,
                CurrencyId = entity.CurrencyId,
                CategoryId = entity?.CategoryId,
                StorageId = entity?.StorageId,
                UserId = entity.UserId,
                Currency = entity.Currency.ToReferenceView()
            };
        }

        public Debt Update(Debt entity, DebtModel model)
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

            return entity;
        }
    }
}
