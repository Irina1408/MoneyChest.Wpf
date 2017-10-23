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
    public class LimitConverter : IEntityModelConverter<Limit, LimitModel>
    {
        public Limit ToEntity(LimitModel model)
        {
            return new Limit()
            {
                DateFrom = model.DateFrom,
                DateUntil = model.DateUntil,
                LimitState = model.LimitState,
                Value = model.Value,
                RemainingValue = model.RemainingValue,
                Remark = model.Remark,
                CurrencyId = model.CurrencyId,
                CategoryId = model?.CategoryId,
                UserId = model.UserId
            };
        }

        public LimitModel ToModel(Limit entity)
        {
            return new LimitModel()
            {
                Id = entity.Id,
                DateFrom = entity.DateFrom,
                DateUntil = entity.DateUntil,
                LimitState = entity.LimitState,
                Value = entity.Value,
                RemainingValue = entity.RemainingValue,
                Remark = entity.Remark,
                CurrencyId = entity.CurrencyId,
                CategoryId = entity?.CategoryId,
                UserId = entity.UserId,
                Currency = entity.Currency.ToReferenceView(),
                Category = entity?.Category?.ToReferenceView()
            };
        }

        public Limit Update(Limit entity, LimitModel model)
        {
            entity.DateFrom = model.DateFrom;
            entity.DateUntil = model.DateUntil;
            entity.LimitState = model.LimitState;
            entity.Value = model.Value;
            entity.RemainingValue = model.RemainingValue;
            entity.Remark = model.Remark;
            entity.CurrencyId = model.CurrencyId;
            entity.CategoryId = model?.CategoryId;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
