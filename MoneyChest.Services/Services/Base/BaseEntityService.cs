using MoneyChest.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IBaseService<T> : IBaseService
        where T : class
    {
        List<T> Get(Func<T, bool> predicate = null);
        T Add(T entity);
        void Delete(T entity);
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
