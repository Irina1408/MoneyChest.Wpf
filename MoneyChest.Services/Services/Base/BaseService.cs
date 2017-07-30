using MoneyChest.Data.Attributes;
using MoneyChest.Data.Context;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IBaseService : IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }

    public interface IBaseService<T> : IBaseService
        where T : class
    {
        List<T> Get(Func<T, bool> predicate = null);
        T Add(T entity);
        void Delete(T entity);
    }

    public abstract class BaseService : IBaseService
    {
        protected ApplicationDbContext _context;
        private List<TrackedItem> trackedItems;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
            trackedItems = new List<TrackedItem>();
        }

        #region IBaseService implementation

        public virtual void SaveChanges()
        {
            TrackChanges();
            _context.SaveChanges();
            UpdateHistory();
            _context.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            TrackChanges();
            await _context.SaveChangesAsync();
            UpdateHistory();
            await _context.SaveChangesAsync();
        }

        private void TrackChanges()
        {
            if (_context.ChangeTracker.HasChanges())
            {
                foreach(var entity in _context.ChangeTracker.Entries().Where(item => item.State == EntityState.Added).ToList())
                    trackedItems.Add(new TrackedItem() { Item = entity.Entity, ActionType = ActionType.Add });

                foreach (var entity in _context.ChangeTracker.Entries().Where(item => item.State == EntityState.Modified).ToList())
                    trackedItems.Add(new TrackedItem() { Item = entity.Entity, ActionType = ActionType.Update });

                foreach (var entity in _context.ChangeTracker.Entries().Where(item => item.State == EntityState.Deleted).ToList())
                    trackedItems.Add(new TrackedItem() { Item = entity.Entity, ActionType = ActionType.Delete });
            }
        }

        private void UpdateHistory()
        {
            // save history for changed items
            foreach(var trackedItem in trackedItems)
            {
                var historyItem = History.HistoryExtensions.CreateHistoryItem(trackedItem.Item, trackedItem.ActionType);
                if (historyItem != null)
                {
                    var historySet = _context.Set(historyItem.GetType());
                    if (historySet != null)
                        historySet.Add(historyItem);
                }
            }

            // cleanup after saving changes
            trackedItems.Clear();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion

        #region Helper classes

        private class TrackedItem
        {
            public object Item { get; set; }
            public ActionType ActionType { get; set; }
        }

        #endregion
    }

    public abstract class BaseService<T> : BaseService, IBaseService<T>
        where T : class
    {
        public BaseService(ApplicationDbContext context) : base(context)
        {
            Entities = context.Set<T>();
        }

        protected DbSet<T> Entities { get; }

        #region IBaseService<T> implementation

        public virtual List<T> Get(Func<T, bool> predicate = null)
        {
            return Entities.Where(predicate).ToList();
        }

        public virtual T Add(T entity)
        {
            return Entities.Add(entity);
        }

        public virtual void Delete(T entity)
        {
            Entities.Remove(entity);
        }

        #endregion
    }
}
