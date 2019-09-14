using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Converters
{
    public class MoneyTransferTemplateConverter : EntityModelConverterBase<MoneyTransferTemplate, MoneyTransferTemplateModel>
    {
        protected override void FillEntity(MoneyTransferTemplate entity, MoneyTransferTemplateModel model)
        {
            entity.Name = model.Name;
            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Value = model.Value;
            entity.Description = model.Description;
            entity.Commission = model.Commission;
            entity.CommissionType = model.CommissionType;
            entity.TakeCommissionFromReceiver = model.TakeCommissionFromReceiver;
            entity.Remark = model.Remark;
            entity.StorageFromId = model.StorageFromId;
            entity.StorageToId = model.StorageToId;
            entity.CategoryId = model.CategoryId;
        }

        protected override void FillModel(MoneyTransferTemplate entity, MoneyTransferTemplateModel model)
        {
            model.Name = entity.Name;
            model.Id = entity.Id;
            model.CurrencyExchangeRate = entity.CurrencyExchangeRate;
            model.Value = entity.Value;
            model.Description = entity.Description;
            model.Commission = entity.Commission;
            model.CommissionType = entity.CommissionType;
            model.TakeCommissionFromReceiver = entity.TakeCommissionFromReceiver;
            model.Remark = entity.Remark;
            model.StorageFromId = entity.StorageFromId;
            model.StorageToId = entity.StorageToId;
            model.CategoryId = entity.CategoryId;
            model.StorageFrom = entity.StorageFrom.ToReferenceView();
            model.StorageTo = entity.StorageTo.ToReferenceView();
            model.StorageFromCurrency = entity.StorageFrom.Currency.ToReferenceView();
            model.StorageToCurrency = entity.StorageTo.Currency.ToReferenceView();
            model.Category = entity?.Category?.ToReferenceView();
        }
    }
}
