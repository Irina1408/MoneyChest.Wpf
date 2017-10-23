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
    public class DebtConverter : IEntityModelConverter<Debt, DebtModel>
    {
        public Debt ToEntity(DebtModel model)
        {
            return new Debt()
            {
                Name = model.Name,
                DebtType = model.DebtType,
                TakingDate = model.TakingDate,
                Value = model.Value,
                PaidValue = model.PaidValue,
                IsRepayed = model.IsRepayed,
                RepayingDate = model?.RepayingDate,
                Remark = model.Remark,
                CurrencyId = model.CurrencyId,
                StorageId = model?.StorageId,
                UserId = model.UserId
            };
        }

        public DebtModel ToModel(Debt entity)
        {
            return new DebtModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                DebtType = entity.DebtType,
                TakingDate = entity.TakingDate,
                Value = entity.Value,
                PaidValue = entity.PaidValue,
                IsRepayed = entity.IsRepayed,
                RepayingDate = entity?.RepayingDate,
                Remark = entity.Remark,
                CurrencyId = entity.CurrencyId,
                StorageId = entity?.StorageId,
                UserId = entity.UserId,
                Currency = entity.Currency.ToReferenceView()
            };
        }

        public Debt Update(Debt entity, DebtModel model)
        {
            entity.Name = model.Name;
            entity.DebtType = model.DebtType;
            entity.TakingDate = model.TakingDate;
            entity.Value = model.Value;
            entity.PaidValue = model.PaidValue;
            entity.IsRepayed = model.IsRepayed;
            entity.RepayingDate = model?.RepayingDate;
            entity.Remark = model.Remark;
            entity.CurrencyId = model.CurrencyId;
            entity.StorageId = model?.StorageId;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
