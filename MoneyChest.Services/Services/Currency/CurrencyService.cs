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
    public interface ICurrencyService : IBaseHistoricizedService<Currency>
    {
    }

    public class CurrencyService : BaseHistoricizedService<Currency>, ICurrencyService
    {
        public CurrencyService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(Currency entity) => entity.UserId;

        protected override Func<Currency, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
