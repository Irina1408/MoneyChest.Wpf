using System;
using System.Collections.Generic;
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

    public interface IBaseService<T>
        where T : class
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T entity);
    }
}
