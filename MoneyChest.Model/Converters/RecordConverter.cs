using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Convert;
using MoneyChest.Model.Model;

namespace MoneyChest.Model.Converters
{
    public class RecordConverter : IEntityModelConverter<Record, RecordModel>
    {
        public Record ToEntity(RecordModel model)
        {
            return new Record()
            {
                Date = model.Date,
                Description = model.Description,
                TransactionType = model.TransactionType,
                Value = model.Value,
                Remark = model.Remark,
                CategoryId = model?.CategoryId,
                CurrencyId = model.CurrencyId,
                StorageId = model?.StorageId,
                DebtId = model?.DebtId,
                UserId = model.UserId
            };
        }

        public RecordModel ToModel(Record entity)
        {
            return new RecordModel()
            {
                Id = entity.Id,
                Date = entity.Date,
                Description = entity.Description,
                TransactionType = entity.TransactionType,
                Value = entity.Value,
                Remark = entity.Remark,
                CategoryId = entity?.CategoryId,
                CurrencyId = entity.CurrencyId,
                StorageId = entity?.StorageId,
                DebtId = entity?.DebtId,
                UserId = entity.UserId,
                Category = entity?.Category?.ToReferenceView(),
                Currency = entity.Currency.ToReferenceView(),
                Storage = entity?.Storage?.ToReferenceView(),
                Debt = entity?.Debt?.ToReferenceView()
            };
        }

        public Record Update(Record entity, RecordModel model)
        {
            entity.Date = model.Date;
            entity.Description = model.Description;
            entity.TransactionType = model.TransactionType;
            entity.Value = model.Value;
            entity.Remark = model.Remark;
            entity.CategoryId = model?.CategoryId;
            entity.CurrencyId = model.CurrencyId;
            entity.StorageId = model?.StorageId;
            entity.DebtId = model?.DebtId;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
