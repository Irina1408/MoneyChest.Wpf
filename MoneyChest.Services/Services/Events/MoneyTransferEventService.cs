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
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services.Events
{
    public interface IMoneyTransferEventService : IBaseIdManagableUserableListService<MoneyTransferEventModel>
    {
    }

    public class MoneyTransferEventService : BaseHistoricizedIdManageableUserableListService<MoneyTransferEvent, MoneyTransferEventModel, MoneyTransferEventConverter>, IMoneyTransferEventService
    {
        public MoneyTransferEventService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<MoneyTransferEvent> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);
    }
}
