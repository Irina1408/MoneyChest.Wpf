using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using System.Data.Entity;
using MoneyChest.Services.Converters;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Services
{
    public interface ILimitService : IIdManagableUserableListServiceBase<LimitModel>
    {
        void RemoveClosed(int userId);
    }

    public class LimitService : HistoricizedIdManageableUserableListServiceBase<Limit, LimitModel, LimitConverter>, ILimitService
    {
        public LimitService(ApplicationDbContext context) : base(context)
        {
        }

        #region ILimitService implementation

        public void RemoveClosed(int userId)
        {
            var limitsToRemove = Entities.Where(x => x.UserId == userId && x.DateUntil < DateTime.Today).ToList();
            limitsToRemove.ForEach(entity => Delete(entity));
            SaveChanges();
        }

        #endregion

        #region Overrides 

        public override LimitModel Add(LimitModel model)
        {
            if (string.IsNullOrEmpty(model.Description))
            {
                var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

            return base.Add(model);
        }

        public override IEnumerable<LimitModel> Add(IEnumerable<LimitModel> models)
        {
            var categoryIds = models.Where(x => x.CategoryId != null).Select(x => x.CategoryId).Distinct().ToList();
            var categories = _context.Categories.Where(x => categoryIds.Contains(x.Id));

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Description)).ToList())
            {
                var category = categories.FirstOrDefault(x => x.Id == model.CategoryId);
                model.Description = category?.Name;
            }

            return base.Add(models);
        }

        public override LimitModel PrepareNew(LimitModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default currency
            var mainCurrency = _context.Currencies.FirstOrDefault(x => x.IsMain && model.UserId == x.UserId);
            model.CurrencyId = mainCurrency?.Id ?? 0;
            model.Currency = mainCurrency?.ToReferenceView();

            return model;
        }

        protected override IQueryable<Limit> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category);

        #endregion
    }
}
