using MoneyChest.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public abstract class BaseService<T> : BaseService//, IBaseService<T>
        where T : class
    {
        public BaseService(ApplicationDbContext context) : base(context)
        {
            Entities = context.Set<T>();
        }

        protected DbSet<T> Entities { get; }

        protected virtual IQueryable<T> Scope => Entities;

        internal virtual T Add(T entity)
        {
            return Entities.Add(entity);
        }

        internal virtual T Update(T entity) => entity;

        internal virtual void Delete(T entity)
        {
            Entities.Remove(entity);
        }
    }
}
