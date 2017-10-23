using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Convert;
using System.Data.Entity;
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services
{
    public interface ICurrencyExchangeRateService : IBaseService<CurrencyExchangeRateModel>, IUserableListService<CurrencyExchangeRateModel>
    {
        List<CurrencyExchangeRateModel> GetList(int userId, int currencyToId);
    }

    public class CurrencyExchangeRateService : BaseHistoricizedService<CurrencyExchangeRate, CurrencyExchangeRateModel, CurrencyExchangeRateConverter>, ICurrencyExchangeRateService
    {
        public CurrencyExchangeRateService(ApplicationDbContext context) : base(context)
        {
        }

        #region ICurrencyExchangeRateService implementation

        public List<CurrencyExchangeRateModel> GetList(int userId, int currencyToId)
        {
            return Scope.Where(item => item.CurrencyFrom.UserId == userId && item.CurrencyTo.UserId == userId && item.CurrencyToId == currencyToId).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region IUserableListService<CurrencyExchangeRateModel> implementation

        public List<CurrencyExchangeRateModel> GetListForUser(int userId)
        {
            return Scope.Where(item => item.CurrencyFrom.UserId == userId && item.CurrencyTo.UserId == userId).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<CurrencyExchangeRate> Scope => Entities.Include(_ => _.CurrencyFrom).Include(_ => _.CurrencyTo);
        protected override CurrencyExchangeRate GetSingleDb(CurrencyExchangeRateModel model) => Entities.FirstOrDefault(e => e.CurrencyFromId == model.CurrencyFromId && e.CurrencyToId == model.CurrencyToId);
        protected override int UserId(CurrencyExchangeRate entity)
        {
            if (entity.CurrencyFrom != null) return entity.CurrencyFrom.UserId;
            if (entity.CurrencyTo != null) return entity.CurrencyTo.UserId;
            return _context.Currencies.FirstOrDefault(item => item.Id == entity.CurrencyFromId).UserId;
        }
        
        #endregion
    }
}
