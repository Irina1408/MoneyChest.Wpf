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
using MoneyChest.Model.Converters;

namespace MoneyChest.Services.Services.Base
{
    public abstract class BaseHistoricizedService<T, TModel, TConverter> : BaseService<T, TModel, TConverter>
        where T : class
        where TModel : class
        where TConverter : IEntityModelConverter<T, TModel>, new()
    {
        private HistoryService _historyService;

        public BaseHistoricizedService(ApplicationDbContext context) : base(context)
        {
            _historyService = new HistoryService(context);
        }

        internal override T Add(T entity)
        {
            entity = base.Add(entity);
            SaveChanges();
            _historyService.WriteHistory(entity, ActionType.Add, UserId(entity));
            return entity;
        }

        internal override T Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Modified)
                _historyService.WriteHistory(entity, ActionType.Update, UserId(entity));
            return base.Update(entity);
        }

        internal override void Delete(T entity)
        {
            _historyService.WriteHistory(entity, ActionType.Delete, UserId(entity));
            base.Delete(entity);
        }

        public override void SaveChanges()
        {
            // save changes
            base.SaveChanges();
            // save history
            _historyService.SaveChanges();
        }

        public override async Task SaveChangesAsync()
        {
            // save changes
            await base.SaveChangesAsync();
            // save history
            await _historyService.SaveChangesAsync();
        }

        protected abstract int UserId(T entity);
    }
}
