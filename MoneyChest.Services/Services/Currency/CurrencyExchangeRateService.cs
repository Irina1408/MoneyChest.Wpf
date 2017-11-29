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
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Services.Converters;

namespace MoneyChest.Services.Services
{
    public interface ICurrencyExchangeRateService : IServiceBase<CurrencyExchangeRateModel>, IUserableListService<CurrencyExchangeRateModel>
    {
        List<CurrencyExchangeRateModel> GetList(int userId, int currencyToId);
        List<CurrencyExchangeRateModel> GetList(List<int> currencyIds);
    }

    public class CurrencyExchangeRateService : HistoricizedServiceBase<CurrencyExchangeRate, CurrencyExchangeRateModel, CurrencyExchangeRateConverter>, ICurrencyExchangeRateService
    {
        public CurrencyExchangeRateService(ApplicationDbContext context) : base(context)
        {
        }

        #region ICurrencyExchangeRateService implementation

        public List<CurrencyExchangeRateModel> GetList(int userId, int currencyToId)
        {
            return Scope.Where(item => item.CurrencyFrom.UserId == userId && item.CurrencyTo.UserId == userId && item.CurrencyToId == currencyToId).ToList().ConvertAll(_converter.ToModel);
        }

        public List<CurrencyExchangeRateModel> GetList(List<int> currencyIds)
        {
            return Scope.Where(item => currencyIds.Contains(item.CurrencyFromId) && currencyIds.Contains(item.CurrencyToId))
                .ToList().ConvertAll(_converter.ToModel);
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
        protected override CurrencyExchangeRate GetDbEntity(CurrencyExchangeRateModel model) => Entities.FirstOrDefault(e => e.CurrencyFromId == model.CurrencyFromId && e.CurrencyToId == model.CurrencyToId);
        protected override CurrencyExchangeRate GetDbDetailedEntity(CurrencyExchangeRate entity) => Scope.FirstOrDefault(e => e.CurrencyFromId == entity.CurrencyFromId && e.CurrencyToId == entity.CurrencyToId);
        protected override List<CurrencyExchangeRate> GetDbEntities(IEnumerable<CurrencyExchangeRateModel> models)
        {
            var result = new List<CurrencyExchangeRate>();
            foreach(var model in models)
            {
                var entity = Entities.FirstOrDefault(e => e.CurrencyFromId == model.CurrencyFromId && e.CurrencyToId == model.CurrencyToId);
                if (entity != null)
                    result.Add(entity);
            }

            return result;
        }
        protected override int UserId(CurrencyExchangeRate entity)
        {
            if (entity.CurrencyFrom != null) return entity.CurrencyFrom.UserId;
            if (entity.CurrencyTo != null) return entity.CurrencyTo.UserId;
            return _context.Currencies.FirstOrDefault(item => item.Id == entity.CurrencyFromId).UserId;
        }
        
        #endregion
    }
}
