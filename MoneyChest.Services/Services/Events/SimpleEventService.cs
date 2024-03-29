﻿using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using System.Data.Entity;
using MoneyChest.Data.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using MoneyChest.Model.Enums;
using MoneyChest.Data.Extensions;
using MoneyChest.Services.Utils;

namespace MoneyChest.Services.Services
{
    public interface ISimpleEventService : IIdManagableUserableListServiceBase<SimpleEventModel>, IEventService<SimpleEventModel>
    {
    }

    public class SimpleEventService : HistoricizedIdManageableUserableListServiceBase<SimpleEvent, SimpleEventModel, SimpleEventConverter>, ISimpleEventService
    {
        private ICurrencyExchangeRateService _currencyExchangeRateService;

        public SimpleEventService(ApplicationDbContext context) : base(context)
        {
            _currencyExchangeRateService = new CurrencyExchangeRateService(context);
        }

        #region ISimpleEventService implementation

        public List<SimpleEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil, bool? autoExecution = null)
        {
            var filter = EventService.GetActiveEventsFilter<SimpleEvent>(userId, dateFrom, dateUntil);
            var autoExecutionFilter = EventService.GetAutoExecutionFilter<SimpleEvent>(autoExecution);

            return EventService.UpdateEventsExchangeRate(_currencyExchangeRateService, 
                Scope.Where(filter).Where(autoExecutionFilter).ToList().ConvertAll(_converter.ToModel));
        }

        #endregion

        #region Overrides

        public override List<SimpleEventModel> GetListForUser(int userId)
        {
            return EventService.UpdateEventsExchangeRate(_currencyExchangeRateService, base.GetListForUser(userId));
        }

        public override SimpleEventModel Add(SimpleEventModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            return base.Add(model);
        }

        public override IEnumerable<SimpleEventModel> Add(IEnumerable<SimpleEventModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            return base.Add(models);
        }

        public override SimpleEventModel PrepareNew(SimpleEventModel model)
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

        protected override IQueryable<SimpleEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Currency).Include(_ => _.Category);

        #endregion
    }
}
