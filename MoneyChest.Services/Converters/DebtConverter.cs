using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Extensions;
using System.Collections.ObjectModel;

namespace MoneyChest.Services.Converters
{
    public class DebtConverter : EntityModelConverterBase<Debt, DebtModel>
    {
        protected override void FillEntity(Debt entity, DebtModel model)
        {
            entity.Description = model.Description;
            entity.DebtType = model.DebtType;

            entity.CurrencyExchangeRate = model.CurrencyExchangeRate;
            entity.Value = model.Value;
            entity.InitialFee = model.InitialFee;
            entity.PaidValue = model.PaidValue;
            entity.OnlyInitialFee = model.OnlyInitialFee;

            entity.PaymentType = model.PaymentType;
            entity.FixedAmount = model.FixedAmount;
            entity.InterestRate = model.InterestRate;
            entity.MonthCount = model.MonthCount;

            entity.TakingDate = model.TakingDate;
            entity.DueDate = model.DueDate;
            entity.RepayingDate = model.RepayingDate;

            entity.IsRepaid = model.IsRepaid;
            entity.Remark = model.Remark;

            entity.CurrencyId = model.CurrencyId;
            entity.CategoryId = model?.CategoryId > 0 ? model?.CategoryId : null;
            entity.StorageId = model?.StorageId > 0 ? model?.StorageId : null;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Debt entity, DebtModel model)
        {
            model.Id = entity.Id;
            model.Description = entity.Description;
            model.DebtType = entity.DebtType;

            model.CurrencyExchangeRate = entity.CurrencyExchangeRate;
            model.Value = entity.Value;
            model.InitialFee = entity.InitialFee;
            model.PaidValue = entity.PaidValue;
            model.OnlyInitialFee = entity.OnlyInitialFee;

            model.PaymentType = entity.PaymentType;
            model.FixedAmount = entity.FixedAmount;
            model.InterestRate = entity.InterestRate;
            model.MonthCount = entity.MonthCount;

            model.TakingDate = entity.TakingDate;
            model.DueDate = entity.DueDate;
            model.RepayingDate = entity.RepayingDate;

            model.IsRepaid = entity.IsRepaid;
            model.Remark = entity.Remark;

            model.CurrencyId = entity.CurrencyId;
            model.CategoryId = entity?.CategoryId;
            model.StorageId = entity?.StorageId;
            model.UserId = entity.UserId;

            model.Currency = entity.Currency?.ToReferenceView();
            model.Category = entity.Category?.ToReferenceView();
            model.Storage = entity.Storage?.ToReferenceView();
            model.Penalties = new ObservableCollection<DebtPenaltyModel>(entity.DebtPenalties.Select(e => new DebtPenaltyModel()
            {
                Id = e.Id,
                Date = e.Date,
                Description = e.Description,
                Value = e.Value,
                DebtId = e.DebtId
            }));
        }
    }
}
