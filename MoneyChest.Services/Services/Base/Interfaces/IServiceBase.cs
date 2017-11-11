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
        T Add(T entity);
        T Update(T entity);
        List<T> Update(IEnumerable<T> entities);
        void Delete(T entity);
        void Delete(IEnumerable<T> entities);
    }
}
