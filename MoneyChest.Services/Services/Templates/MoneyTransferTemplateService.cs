using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using System.Data.Entity;
using MoneyChest.Model.Model;
using MoneyChest.Model.Extensions;
using MoneyChest.Services.Converters;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Utils;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferTemplateService : IIdManagableServiceBase<MoneyTransferTemplateModel>, IUserableListService<MoneyTransferTemplateModel>
    {
        MoneyTransferModel Create(MoneyTransferTemplateModel model, Action<MoneyTransferModel> overrides = null);
        MoneyTransferTemplateModel Create(MoneyTransferModel model, Action<MoneyTransferTemplateModel> overrides = null);
    }

    public class MoneyTransferTemplateService : HistoricizedIdManageableServiceBase<MoneyTransferTemplate, MoneyTransferTemplateModel, MoneyTransferTemplateConverter>, IMoneyTransferTemplateService
    {
        public MoneyTransferTemplateService(ApplicationDbContext context) : base(context)
        {
        }

        #region IMoneyTransferTemplateService implementation

        public MoneyTransferTemplateModel Create(MoneyTransferModel model, Action<MoneyTransferTemplateModel> overrides = null)
        {
            var moneyTransfer = new MoneyTransferTemplateModel()
            {
                Name = model.Description,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId,
                CategoryId = model.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Description = model.Description,
                Remark = model.Remark,
                TakeCommissionFromReceiver = model.TakeCommissionFromReceiver,
                Value = model.Value,
                StorageFrom = model.StorageFrom,
                StorageFromCurrency = model.StorageFromCurrency,
                StorageTo = model.StorageTo,
                StorageToCurrency = model.StorageToCurrency,
                StorageFromValue = model.StorageFromValue,
                StorageToValue = model.StorageToValue,
                Category = model.Category
            };

            overrides?.Invoke(moneyTransfer);
            return moneyTransfer;
        }

        public MoneyTransferModel Create(MoneyTransferTemplateModel model, Action<MoneyTransferModel> overrides = null)
        {
            var moneyTransfer = new MoneyTransferModel()
            {
                Date = DateTime.Now,
                StorageFromId = model.StorageFromId,
                StorageToId = model.StorageToId,
                CategoryId = model.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                Description = model.Description,
                Remark = model.Remark,
                TakeCommissionFromReceiver = model.TakeCommissionFromReceiver,
                Value = model.Value,
                StorageFrom = model.StorageFrom,
                StorageFromCurrency = model.StorageFromCurrency,
                StorageTo = model.StorageTo,
                StorageToCurrency = model.StorageToCurrency,
                StorageFromValue = model.StorageFromValue,
                StorageToValue = model.StorageToValue,
                Category = model.Category
            };

            overrides?.Invoke(moneyTransfer);
            return moneyTransfer;
        }

        #endregion

        #region IUserableListService<MoneyTransferTemplateModel> implementation

        public List<MoneyTransferTemplateModel> GetListForUser(int userId) =>
            Scope.Where(item => item.StorageFrom.UserId == userId && item.StorageTo.UserId == userId).ToList().ConvertAll(_converter.ToModel);

        #endregion

        #region Overrides

        public override MoneyTransferTemplateModel Add(MoneyTransferTemplateModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            if (string.IsNullOrEmpty(model.Name))
                model.Name = model.Description;

            return base.Add(model);
        }

        public override IEnumerable<MoneyTransferTemplateModel> Add(IEnumerable<MoneyTransferTemplateModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Name)).ToList())
            {
                model.Name = model.Description;
            }

            return base.Add(models);
        }

        protected override IQueryable<MoneyTransferTemplate> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);

        protected override int UserId(MoneyTransferTemplate entity)
        {
            if (entity.StorageFrom != null) return entity.StorageFrom.UserId;
            if (entity.StorageTo != null) return entity.StorageTo.UserId;
            return _context.Storages.FirstOrDefault(item => item.Id == entity.StorageFromId).UserId;
        }

        #endregion
    }
}
