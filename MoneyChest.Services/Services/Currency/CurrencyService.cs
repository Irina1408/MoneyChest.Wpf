using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Data.Enums;

namespace MoneyChest.Services.Services
{
    public interface ICurrencyService : IBaseHistoricizedService<Currency>, IIdManageable<Currency>
    {
    }

    public class CurrencyService : BaseHistoricizedService<Currency>, ICurrencyService
    {
        #region Initialization

        public CurrencyService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region Overrides

        protected override int UserId(Currency entity) => entity.UserId;

        protected override Expression<Func<Currency, bool>> LimitByUser(int userId) => item => item.UserId == userId;

        #endregion

        #region IIdManageable<T> implementation

        public Currency Get(int id) => Entities.FirstOrDefault(_ => _.Id == id);

        public void Delete(int id) => Delete(Get(id));

        #endregion
    }
}
