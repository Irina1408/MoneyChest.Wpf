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
    public class MoneyTransferConverter : EntityModelConverterBase<MoneyTransfer, MoneyTransferModel>
    {
        protected override void FillEntity(MoneyTransfer entity, MoneyTransferModel model)
        {
            entity.Date = model.Date;
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

        protected override void FillModel(MoneyTransfer entity, MoneyTransferModel model)
        {
            model.Id = entity.Id;
            model.Date = entity.Date;
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
