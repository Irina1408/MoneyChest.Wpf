using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Utils;
using MoneyChest.Model.Model;
using System.Data.Entity;
using MoneyChest.Model.Extensions;
using MoneyChest.Services.Converters;
using MoneyChest.Model.Enums;
using MoneyChest.Data.Extensions;

namespace MoneyChest.Services.Services
{
    public interface IRecordTemplateService : IIdManagableUserableListServiceBase<RecordTemplateModel>
    {
        RecordModel Create(RecordTemplateModel model, Action<RecordModel> overrides = null);
        RecordTemplateModel Create(RecordModel model, Action<RecordTemplateModel> overrides = null);
    }

    public class RecordTemplateService : HistoricizedIdManageableUserableListServiceBase<RecordTemplate, RecordTemplateModel, RecordTemplateConverter>, IRecordTemplateService
    {
        #region Initialization

        public RecordTemplateService(ApplicationDbContext context) : base(context)
        {
        }

        #endregion

        #region IRecordTemplateService implementation
        
        public RecordModel Create(RecordTemplateModel model, Action<RecordModel> overrides = null)
        {
            var record = new RecordModel()
            {
                Date = DateTime.Now,
                CategoryId = model.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                CurrencyId = model.CurrencyId,
                Description = model.Description,
                RecordType = model.RecordType,
                StorageId = model.StorageId,
                Value = model.Value,
                UserId = model.UserId,
                Remark = model.Remark,
                Currency = model.Currency,
                Storage = model.Storage,
                Category = model.Category,
                EventId = model.Id
            };

            overrides?.Invoke(record);
            return record;
        }

        public RecordTemplateModel Create(RecordModel model, Action<RecordTemplateModel> overrides = null)
        {
            var record = new RecordTemplateModel()
            {
                Name = model.Description,
                CategoryId = model.CategoryId,
                Commission = model.Commission,
                CommissionType = model.CommissionType,
                CurrencyExchangeRate = model.CurrencyExchangeRate,
                CurrencyId = model.Currency.Id,
                Description = model.Description,
                RecordType = model.RecordType,
                StorageId = model.StorageId,
                Value = model.Value,
                UserId = model.UserId,
                Remark = model.Remark,
                DebtId = model.DebtId,
                Debt = model.Debt,
                Storage = model.Storage,
                Category = model.Category
            };

            overrides?.Invoke(record);
            return record;
        }

        #endregion

        #region Overrides

        public override RecordTemplateModel Add(RecordTemplateModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            if (string.IsNullOrEmpty(model.Name))
                model.Name = model.Description;

            return base.Add(model);
        }

        public override IEnumerable<RecordTemplateModel> Add(IEnumerable<RecordTemplateModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            foreach (var model in models.Where(x => string.IsNullOrEmpty(x.Name)).ToList())
            {
                model.Name = model.Description;
            }

            return base.Add(models);
        }

        public override RecordTemplateModel PrepareNew(RecordTemplateModel model)
        {
            // base preparing
            base.PrepareNew(model);
            // set default currency
            var mainCurrency = _context.Currencies.FirstOrDefault(x => x.IsMain && model.UserId == x.UserId);
            model.CurrencyId = mainCurrency?.Id ?? 0;
            model.Currency = mainCurrency?.ToReferenceView();

            // set default storage
            var storage = _context.Storages.FirstOrDefault(x => x.CurrencyId == model.CurrencyId && model.UserId == x.UserId);
            model.StorageId = storage?.Id ?? 0;
            model.Storage = storage?.ToReferenceView();

            return model;
        }
        
        protected override IQueryable<RecordTemplate> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.Debt);

        #endregion
    }
}
