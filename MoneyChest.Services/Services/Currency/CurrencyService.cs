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
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Model.Model;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services
{
    public interface ICurrencyService : IBaseIdManagableUserableListService<CurrencyModel>
    {
        CurrencyModel GetMain(int userId);
        void SetMain(int userId, int currencyId);
        List<CurrencyModel> GetUsed(int userId);
    }

    public class CurrencyService : BaseHistoricizedIdManageableUserableListService<Currency, CurrencyModel, CurrencyConverter>, ICurrencyService
    {
        #region Initialization

        public CurrencyService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region ICurrencyService implementation

        public CurrencyModel GetMain(int userId)
        {
            return _converter.ToModel(Entities.FirstOrDefault(e => e.UserId == userId && e.IsMain));
        }

        public void SetMain(int userId, int currencyId)
        {
            Entities.Where(e => e.UserId == userId).ToList().ForEach(c => c.IsMain = c.Id == currencyId);
            SaveChanges();
        }

        public List<CurrencyModel> GetUsed(int userId)
        {
            return Scope.Where(e => e.IsUsed || e.Records.Any() || e.SimpleEvents.Any() || e.Storages.Any() || e.Limits.Any() || e.Debts.Any())
                .ToList().ConvertAll(_converter.ToModel);
        }

        #endregion
    }
}
