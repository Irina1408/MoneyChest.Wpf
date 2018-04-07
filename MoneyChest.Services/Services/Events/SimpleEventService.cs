using MoneyChest.Data.Entities;
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

        public List<SimpleEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil)
        {
            var filter = EventService.GetActiveEventsFilter<SimpleEvent>(userId, dateFrom, dateUntil);
            return Scope.Where(filter).ToList().ConvertAll(_converter.ToModel);
        }

        #endregion

        #region Overrides

        protected override IQueryable<SimpleEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Currency).Include(_ => _.Category);

        #endregion

        #region Private methods

        //private void UpdateCurrencyExchangeRate(List<SimpleEventModel> events)
        //{
        //    var requiredToUpdateEvents = events.Where(x => x.IsCurrencyExchangeRateRequired && x.TakeExistingCurrencyExchangeRate).ToList();
        //    if (requiredToUpdateEvents.Count > 0)
        //    {
        //        // load currency exchange rates

        //    }
        //}

        #endregion
    }
}
