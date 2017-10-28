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
    public class CurrencyConverter : IEntityModelConverter<Currency, CurrencyModel>
    {
        public Currency ToEntity(CurrencyModel model)
        {
            return new Currency()
            {
                Name = model.Name,
                Code = model.Code,
                Symbol = model.Symbol,
                IsUsed = model.IsUsed,
                IsMain = model.IsMain,
                SymbolAlignmentIsRight = model.SymbolAlignmentIsRight,
                UserId = model.UserId
            };
        }

        public CurrencyModel ToModel(Currency entity)
        {
            return new CurrencyModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Symbol = entity.Symbol,
                IsUsed = entity.IsUsed,
                IsMain = entity.IsMain,
                SymbolAlignmentIsRight = entity.SymbolAlignmentIsRight,
                UserId = entity.UserId
            };
        }

        public Currency Update(Currency entity, CurrencyModel model)
        {
            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.Symbol = model.Symbol;
            entity.IsUsed = model.IsUsed;
            entity.IsMain = model.IsMain;
            entity.SymbolAlignmentIsRight = model.SymbolAlignmentIsRight;
            entity.UserId = model.UserId;

            return entity;
        }
    }
}
