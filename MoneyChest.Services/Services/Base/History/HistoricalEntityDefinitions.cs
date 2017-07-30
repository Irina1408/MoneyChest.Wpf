using MoneyChest.Data.Entities;
using MoneyChest.Data.Entities.History;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base.History
{
    public static class HistoricalEntityDefinitions
    {
        internal static void DefineHistoricizedEntities(this HistoricalEntityFactory f)
        {
            f.Define<Category, CategoryHistory>((entity, entityHistory) =>
            {
                entityHistory.Id = entity.Id;
                entityHistory.InHistory = entity.InHistory;
                entityHistory.Name = entity.Name;
                entityHistory.ParentCategoryId = entity.ParentCategoryId;
                entityHistory.TransactionType = entity.TransactionType;
                entityHistory.UserId = entity.UserId;
            });

            f.Define<Currency, CurrencyHistory>((entity, entityHistory) =>
            {
                entityHistory.Id = entity.Id;
                entityHistory.Code = entity.Code;
                entityHistory.Name = entity.Name;
                entityHistory.IsMain = entity.IsMain;
                entityHistory.IsUsed = entity.IsUsed;
                entityHistory.Symbol = entity.Symbol;
                entityHistory.UserId = entity.UserId;
            });

            f.Define<CurrencyExchangeRate, CurrencyExchangeRateHistory>((entity, entityHistory) =>
            {
                entityHistory.CurrencyFromId = entity.CurrencyFromId;
                entityHistory.CurrencyToId = entity.CurrencyToId;
                entityHistory.Rate = entity.Rate;
                entityHistory.UserId = entity.CurrencyFrom.UserId;
            });

            f.Define<MoneyTransferEvent, MoneyTransferEventHistory>((entity, entityHistory) =>
            {
                
            });
        }
    }
}
