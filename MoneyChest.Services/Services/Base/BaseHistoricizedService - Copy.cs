using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Enums;
using MoneyChest.Data.Entities.History;
using System.Data.Entity;

namespace MoneyChest.Services.Services.Base
{
    internal interface IBaseHistoricizedService<T, THistory> : IBaseService<T>
        where T : class
        where THistory : class
    {
        void UpdateHistory(T entity, ActionType actionType, Action<THistory> overrides = null);
    }

    internal abstract class BaseHistoricizedService<T, THistory> : BaseService<T>, IBaseHistoricizedService<T, THistory>
        where T : class
        where THistory : class, IUserActionHistory, new()
    {
        public BaseHistoricizedService(ApplicationDbContext context) : base(context)
        {
            History = context.Set<THistory>();
        }

        protected DbSet<THistory> History { get; }

        public override T Add(T entity)
        {
            UpdateHistory(entity, ActionType.Add);
            return base.Add(entity);
        }

        public override void Delete(T entity)
        {
            UpdateHistory(entity, ActionType.Delete);
            base.Delete(entity);
        }

        public override void SaveChanges()
        {
            if(_context.ChangeTracker.HasChanges())
            {
                foreach(var entity in _context.ChangeTracker.Entries<T>()
                    .Where(item => item.State == EntityState.Modified)
                    .Select(item => item.Entity).ToList())
                {
                    UpdateHistory(entity, ActionType.Update);
                }
            }

            base.SaveChanges();
        }

        public override async Task SaveChangesAsync()
        {
            if (_context.ChangeTracker.HasChanges())
            {
                foreach (var entity in _context.ChangeTracker.Entries<T>()
                    .Where(item => item.State == EntityState.Modified)
                    .Select(item => item.Entity).ToList())
                {
                    UpdateHistory(entity, ActionType.Update);
                }
            }

            await base.SaveChangesAsync();
        }

        public virtual void UpdateHistory(T entity, ActionType actionType, Action<THistory> overrides = null)
        {
            var historyItem = new THistory()
            {
                ActionDateTime = DateTime.Now,
                ActionType = actionType
            };

            // TODO: fill historical entity

            History.Add(historyItem);
        }
    }
}
