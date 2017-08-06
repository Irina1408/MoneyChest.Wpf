using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;

namespace MoneyChest.Services.Services.Base
{
    public interface IBaseUserableService<T> : IBaseService<T>
        where T : class
    {
        List<T> GetAllForUser(int userId, Expression<Func<T, bool>> expression = null);
        T GetForUser(int userId, Expression<Func<T, bool>> expression = null);
    }

    public abstract class BaseUserableService<T> : BaseService<T>, IBaseUserableService<T>
        where T : class
    {
        public BaseUserableService(ApplicationDbContext context) : base(context)
        {
        }
        
        public virtual List<T> GetAllForUser(int userId, Expression<Func<T, bool>> expression = null)
        {
            if (expression == null) expression = item => true;
            return Entities.Where(LimitByUser(userId)).Where(expression).ToList();
        }

        public virtual T GetForUser(int userId, Expression<Func<T, bool>> expression = null)
        {
            if (expression == null) expression = item => true;
            return Entities.Where(LimitByUser(userId)).FirstOrDefault(expression);
        }

        protected abstract int UserId(T entity);

        protected abstract Expression<Func<T, bool>> LimitByUser(int userId);
    }
}
