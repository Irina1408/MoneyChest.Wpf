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
    public interface ICurrencyService : IIdManagableUserableListServiceBase<CurrencyModel>
    {
        CurrencyModel GetMain(int userId);
        // TODO: should be removed
        void SetMain(int userId, int currencyId);
        void SetMain(CurrencyModel model);
        List<CurrencyModel> GetUsed(int userId);
    }

    public class CurrencyService : HistoricizedIdManageableUserableListServiceBase<Currency, CurrencyModel, CurrencyConverter>, ICurrencyService
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
            SetMainInternal(userId, currencyId);
            SaveChanges();
        }

        public void SetMain(CurrencyModel model)
        {
            // update model
            model.IsMain = true;
            // set new main currency
            SetMain(model.UserId, model.Id);
        }

        public List<CurrencyModel> GetUsed(int userId)
        {
            return Scope.Where(e => e.IsUsed || e.Records.Any() || e.SimpleEvents.Any() || e.Storages.Any() || e.Limits.Any() || e.Debts.Any())
                .ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        internal override Currency Add(Currency entity)
        {
            entity = base.Add(entity);

            // check new currency is main
            if (entity.IsMain)
                SetMain(entity.UserId, entity.Id);

            return entity;
        }

        internal override Currency Update(Currency entity)
        {
            var isMainOriginal = _context.Entry(entity).OriginalValues.GetValue<bool>(nameof(Currency.IsMain));
            var isMainCurrent = _context.Entry(entity).CurrentValues.GetValue<bool>(nameof(Currency.IsMain));

            if (isMainOriginal != isMainCurrent)
            {
                // if currency was updated to main, set new main, else revert main currency
                if (isMainCurrent)
                    SetMainInternal(entity.UserId, entity.Id);
                else
                    entity.IsMain = true;
            }
            return base.Update(entity);
        }

        internal override void Delete(Currency entity)
        {
            if (entity.IsMain)
                throw new Exception("Main currency cannot be removed");
            
            base.Delete(entity);
        }

        #endregion

        #region Private methods

        private void SetMainInternal(int userId, int currencyId)
        {
            // set current main currency as not main
            Entities.Where(e => e.UserId == userId && e.IsMain)
                .ToList()
                .ForEach(entity =>
                {
                    entity.IsMain = false;
                    base.Update(entity);
                });

            // set new main currency
            var newMain = Entities.FirstOrDefault(e => e.Id == currencyId);
            newMain.IsMain = true;
            // update history
            base.Update(newMain);
        }

        #endregion
    }
}
