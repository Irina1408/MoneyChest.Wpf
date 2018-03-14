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
        public SimpleEventService(ApplicationDbContext context) : base(context)
        {
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
    }
}
