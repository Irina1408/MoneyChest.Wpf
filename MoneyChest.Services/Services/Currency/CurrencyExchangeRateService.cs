using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public interface ICurrencyExchangeRateService : IBaseHistoricizedService<CurrencyExchangeRate>
    {
    }

    public class CurrencyExchangeRateService : BaseHistoricizedService<CurrencyExchangeRate>, ICurrencyExchangeRateService
    {
        public CurrencyExchangeRateService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(CurrencyExchangeRate entity)
        {
            if (entity.CurrencyFrom != null) return entity.CurrencyFrom.UserId;
            if (entity.CurrencyTo != null) return entity.CurrencyTo.UserId;
            return _context.Currencies.FirstOrDefault(item => item.Id == entity.CurrencyFromId).UserId;
        }

        protected override Func<CurrencyExchangeRate, bool> LimitByUser(int userId) =>
            item => item.CurrencyFrom.UserId == userId && item.CurrencyTo.UserId == userId;
    }
}
