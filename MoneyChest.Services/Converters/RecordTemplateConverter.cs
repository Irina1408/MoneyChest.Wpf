using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Converters
{
    public class RecordTemplateConverter : EntityModelConverterBase<RecordTemplate, RecordTemplateModel>
    {
        protected override void FillEntity(RecordTemplate entity, RecordTemplateModel model)
        {
            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.RecordType = model.RecordType;
            entity.Value = model.Value;
            entity.Remark = model.Remark;
            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Commission = model.Commission;
            entity.CommissionType = model.CommissionType;
            entity.CategoryId = model?.CategoryId;
            entity.CurrencyId = model.CurrencyId;
            entity.StorageId = model.StorageId;
            entity.DebtId = model.DebtId;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(RecordTemplate entity, RecordTemplateModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.RecordType = entity.RecordType;
            model.Value = entity.Value;
            model.Remark = entity.Remark;
            model.CurrencyExchangeRate = entity.CurrencyExchangeRate;
            model.Commission = entity.Commission;
            model.CommissionType = entity.CommissionType;
            model.CategoryId = entity.CategoryId;
            model.CurrencyId = entity.CurrencyId;
            model.StorageId = entity.StorageId;
            model.DebtId = entity.DebtId;
            model.UserId = entity.UserId;
            model.Category = entity?.Category?.ToReferenceView();
            model.Currency = entity.Currency.ToReferenceView();
            model.Storage = entity?.Storage?.ToReferenceView();
            model.Debt = entity?.Debt?.ToReferenceView();
        }
    }
}
