﻿using System;
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
    public class RecordConverter : EntityModelConverterBase<Record, RecordModel>
    {
        protected override void FillEntity(Record entity, RecordModel model)
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
        }

        protected override void FillModel(Record entity, RecordModel model)
        {
            model.Id = entity.Id;
            model.Date = entity.Date;
            model.Description = entity.Description;
            model.TransactionType = entity.TransactionType;
            model.Value = entity.Value;
            model.Remark = entity.Remark;
            model.CategoryId = entity?.CategoryId;
            model.CurrencyId = entity.CurrencyId;
            model.StorageId = entity?.StorageId;
            model.DebtId = entity?.DebtId;
            model.UserId = entity.UserId;
            model.Category = entity?.Category?.ToReferenceView();
            model.Currency = entity.Currency.ToReferenceView();
            model.Storage = entity?.Storage?.ToReferenceView();
            model.Debt = entity?.Debt?.ToReferenceView();
        }
    }
}