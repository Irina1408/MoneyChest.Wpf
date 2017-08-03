using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Enums;
using MoneyChest.Data.Entities.History;
using System.Data.Entity;
using MoneyChest.Services.Services.History;

namespace MoneyChest.Services.Services.Base
{
    public abstract class BaseHistoricizedService<T> : BaseService<T>
        where T : class
    {
        internal HistoryService _historyService;

        public BaseHistoricizedService(ApplicationDbContext context) : base(context)
        {
            _historyService = new HistoryService(context);
        }

        public override void Delete(T entity)
        {
            _historyService.WriteHistory(entity, ActionType.Delete, UserId(entity));
            base.Delete(entity);
        }

        public override void SaveChanges()
        {
            base.SaveChanges();
            // save history
            _historyService.SaveChanges();
        }

        public override async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
            // save history
            await _historyService.SaveChangesAsync();
        }

        protected abstract int UserId(T entity);
    }
}
