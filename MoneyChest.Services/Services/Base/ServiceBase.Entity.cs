using MoneyChest.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public abstract class ServiceBase<T> : ServiceBase//, IBaseService<T>
        where T : class
    {
        public ServiceBase(ApplicationDbContext context) : base(context)
        {
            Entities = context.Set<T>();
        }

        protected DbSet<T> Entities { get; }

        protected virtual IQueryable<T> Scope => Entities;

        internal virtual T Add(T entity)
        {
            return Entities.Add(entity);
        }

        internal virtual IEnumerable<T> Add(IEnumerable<T> entities)
        {
            return Entities.AddRange(entities);
        }

        internal virtual T Update(T entity) => entity;

        internal virtual void Delete(T entity)
        {
            Entities.Remove(entity);
        }
    }
}
