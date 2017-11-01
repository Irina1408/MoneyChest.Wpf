using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Data.Converters
{
    public class CurrencyExchangeRateConverter : IEntityModelConverter<CurrencyExchangeRate, CurrencyExchangeRateModel>
    {
        public CurrencyExchangeRate ToEntity(CurrencyExchangeRateModel model)
        {
            return new CurrencyExchangeRate()
            {
                Rate = model.Rate,
                CurrencyFromId = model.CurrencyFromId,
                CurrencyToId = model.CurrencyToId
            };
        }

        public CurrencyExchangeRateModel ToModel(CurrencyExchangeRate entity)
        {
            return new CurrencyExchangeRateModel()
            {
                Rate = entity.Rate,
                CurrencyFromId = entity.CurrencyFromId,
                CurrencyToId = entity.CurrencyToId,
                CurrencyFrom = entity.CurrencyFrom.ToReferenceView(),
                CurrencyTo = entity.CurrencyTo.ToReferenceView()
            };
        }

        public CurrencyExchangeRate Update(CurrencyExchangeRate entity, CurrencyExchangeRateModel model)
        {
            entity.Rate = model.Rate;
            entity.CurrencyFromId = model.CurrencyFromId;
            entity.CurrencyToId = model.CurrencyToId;

            return entity;
        }
    }
}
