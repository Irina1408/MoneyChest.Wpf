using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Extensions;
using MoneyChest.Model.Model;

namespace MoneyChest.Data.Converters
{
    public class CurrencyConverter : EntityModelConverterBase<Currency, CurrencyModel>
    {
        protected override void FillEntity(Currency entity, CurrencyModel model)
        {
            entity.Name = model.Name;
            entity.Code = model.Code;
            entity.Symbol = model.Symbol;
            entity.IsUsed = model.IsUsed;
            entity.IsMain = model.IsMain;
            entity.SymbolAlignmentIsRight = model.SymbolAlignmentIsRight;
            entity.UserId = model.UserId;
        }

        protected override void FillModel(Currency entity, CurrencyModel model)
        {
            model.Id = entity.Id;
            model.Name = entity.Name;
            model.Code = entity.Code;
            model.Symbol = entity.Symbol;
            model.IsUsed = entity.IsUsed;
            model.IsMain = entity.IsMain;
            model.SymbolAlignmentIsRight = entity.SymbolAlignmentIsRight;
            model.UserId = entity.UserId;
        }
    }
}
