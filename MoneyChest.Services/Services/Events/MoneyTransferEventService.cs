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
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using MoneyChest.Services.Utils;

namespace MoneyChest.Services.Services
{
    public interface IMoneyTransferEventService : IIdManagableUserableListServiceBase<MoneyTransferEventModel>, IEventService<MoneyTransferEventModel>
    {
    }

    public class MoneyTransferEventService : HistoricizedIdManageableUserableListServiceBase<MoneyTransferEvent, MoneyTransferEventModel, MoneyTransferEventConverter>, IMoneyTransferEventService
    {
        private ICurrencyExchangeRateService _currencyExchangeRateService;

        public MoneyTransferEventService(ApplicationDbContext context) : base(context)
        {
            _currencyExchangeRateService = new CurrencyExchangeRateService(context);
        }

        #region IMoneyTransferEventService implementation

        public List<MoneyTransferEventModel> GetActiveForPeriod(int userId, DateTime dateFrom, DateTime dateUntil, bool? autoExecution = null)
        {
            var filter = EventService.GetActiveEventsFilter<MoneyTransferEvent>(userId, dateFrom, dateUntil);
            var autoExecutionFilter = EventService.GetAutoExecutionFilter<MoneyTransferEvent>(autoExecution);

            return EventService.UpdateEventsExchangeRate(_currencyExchangeRateService, 
                Scope.Where(filter).Where(autoExecutionFilter).ToList().ConvertAll(_converter.ToModel));
        }

        #endregion

        #region Overrides

        public override List<MoneyTransferEventModel> GetListForUser(int userId)
        {
            return EventService.UpdateEventsExchangeRate(_currencyExchangeRateService, base.GetListForUser(userId));
        }

        public override MoneyTransferEventModel Add(MoneyTransferEventModel model)
        {
            // update description from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, model);

            return base.Add(model);
        }

        public override IEnumerable<MoneyTransferEventModel> Add(IEnumerable<MoneyTransferEventModel> models)
        {
            // update descriptions from category if it wasn't populated
            ServiceHelper.UpdateDescription(_context, models);

            return base.Add(models);
        }

        protected override IQueryable<MoneyTransferEvent> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);

        #endregion
    }
}
