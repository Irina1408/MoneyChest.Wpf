using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IServiceBase : IDisposable
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }

    public interface IServiceBase<T>
        where T : class
    {
        T PrepareNew(T entity);
        T Add(T entity);
        IEnumerable<T> Add(IEnumerable<T> entities);
        T Update(T entity);
        IEnumerable<T> Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}
