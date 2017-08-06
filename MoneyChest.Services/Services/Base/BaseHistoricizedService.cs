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
    public interface IBaseHistoricizedService<T> : IBaseUserableService<T>
        where T : class
    {
    }

    public abstract class BaseHistoricizedService<T> : BaseUserableService<T>, IBaseHistoricizedService<T>
        where T : class
    {
        internal HistoryService _historyService;

        public BaseHistoricizedService(ApplicationDbContext context) : base(context)
        {
            _historyService = new HistoryService(context);
        }

        public override T Add(T entity)
        {
            _historyService.WriteHistory(entity, ActionType.Add, UserId(entity));
            return base.Add(entity);
        }

        public override void Delete(T entity)
        {
            _historyService.WriteHistory(entity, ActionType.Delete, UserId(entity));
            base.Delete(entity);
        }

        public override void SaveChanges()
        {
            // save changed items
            foreach (var entity in _context.ChangeTracker.Entries<T>()
                    .Where(item => item.State == EntityState.Modified)
                    .Select(item => item.Entity).ToList())
            {
                _historyService.WriteHistory(entity, ActionType.Update, UserId(entity));
            }

            base.SaveChanges();
            // save history
            _historyService.SaveChanges();
        }

        public override async Task SaveChangesAsync()
        {
            // save changed items
            foreach (var entity in _context.ChangeTracker.Entries<T>()
                    .Where(item => item.State == EntityState.Modified)
                    .Select(item => item.Entity).ToList())
            {
                _historyService.WriteHistory(entity, ActionType.Update, UserId(entity));
            }

            await base.SaveChangesAsync();
            // save history
            await _historyService.SaveChangesAsync();
        }
    }
}
