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
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services.Events
{
    public interface IEventService
    {
        EventsScopeModel Get(List<int> ids);
    }

    public class EventService : ServiceBase, IEventService
    {
        #region Private fields

        private MoneyTransferEventConverter _moneyTransferEventConverter;
        private RepayDebtEventConverter _repayDebtEventConverter;
        private SimpleEventConverter _simpleEventConverter;

        #endregion

        #region Initialization

        public EventService(ApplicationDbContext context) : base(context)
        {
            _moneyTransferEventConverter = new MoneyTransferEventConverter();
            _repayDebtEventConverter = new RepayDebtEventConverter();
            _simpleEventConverter = new SimpleEventConverter();
        }

        #endregion

        #region IEventService implementation

        public EventsScopeModel Get(List<int> ids)
        {
            // TODO: includes
            return new EventsScopeModel()
            {
                MoneyTransferEvents = _context.MoneyTransferEvents
                    .Include(_ => _.StorageFrom).Include(_ => _.StorageTo).Include(_ => _.Category)
                    .Where(_ => ids.Contains(_.Id)).ToList().ConvertAll(_moneyTransferEventConverter.ToModel),

                RepayDebtEvents = _context.RepayDebtEvents
                    .Include(_ => _.Storage).Include(_ => _.Debt)
                    .Where(_ => ids.Contains(_.Id)).ToList().ConvertAll(_repayDebtEventConverter.ToModel),

                SimpleEvents = _context.SimpleEvents
                    .Include(_ => _.Storage).Include(_ => _.Currency).Include(_ => _.Category)
                    .Where(_ => ids.Contains(_.Id)).ToList().ConvertAll(_simpleEventConverter.ToModel)
            };
        }

        #endregion
    }
}
