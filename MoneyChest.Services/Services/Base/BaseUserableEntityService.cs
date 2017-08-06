using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Base
{
    public interface IBaseUserableService<T> : IBaseService<T>
        where T : class
    {
        List<T> GetAllForUser(int userId, Func<T, bool> predicate = null);
        T GetForUser(int userId, Func<T, bool> predicate = null);
    }

    public abstract class BaseUserableService<T> : BaseService<T>, IBaseUserableService<T>
        where T : class
    {
        public BaseUserableService(ApplicationDbContext context) : base(context)
        {
        }
        
        public virtual List<T> GetAllForUser(int userId, Func<T, bool> predicate = null)
        {
            return Entities.Where(item => LimitByUser(userId)(item) && (predicate == null || predicate.Invoke(item))).ToList();
        }

        public virtual T GetForUser(int userId, Func<T, bool> predicate = null)
        {
            return Entities.FirstOrDefault(item => LimitByUser(userId)(item) && (predicate == null || predicate.Invoke(item)));
        }

        protected abstract int UserId(T entity);

        protected abstract Func<T, bool> LimitByUser(int userId);
    }
}
