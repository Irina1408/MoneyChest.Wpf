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
    public class CurrencyExchangeRateConverter : EntityModelConverterBase<CurrencyExchangeRate, CurrencyExchangeRateModel>
    {
        protected override void FillEntity(CurrencyExchangeRate entity, CurrencyExchangeRateModel model)
        {
            entity.Rate = model.Rate;
            entity.SwappedCurrencies = model.SwappedCurrencies;
            entity.CurrencyFromId = model.CurrencyFromId;
            entity.CurrencyToId = model.CurrencyToId;
        }

        protected override void FillModel(CurrencyExchangeRate entity, CurrencyExchangeRateModel model)
        {
            model.Rate = entity.Rate;
            model.SwappedCurrencies = entity.SwappedCurrencies;
            model.CurrencyFromId = entity.CurrencyFromId;
            model.CurrencyToId = entity.CurrencyToId;
            model.CurrencyFrom = entity.CurrencyFrom.ToReferenceView();
            model.CurrencyTo = entity.CurrencyTo.ToReferenceView();
        }
    }
}
