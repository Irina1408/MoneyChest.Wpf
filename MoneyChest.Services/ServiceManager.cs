using MoneyChest.Data.Context;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services
{
    public static class ServiceManager
    {
        private static ContextProvider _contextProvider;
        
        public static TService ConfigureService<TService>()
            where TService : ServiceBase
        {
            if (_contextProvider == null) _contextProvider = new ContextProvider();
            return Activator.CreateInstance(typeof(TService), _contextProvider.Context) as TService;
        }

        public static void Dispose()
        {
            if(_contextProvider != null)
            {
                _contextProvider.Dispose();
                _contextProvider = null;
            }
        }
    }

    public class ContextProvider : IDisposable
    {
        public ContextProvider() => Context = ApplicationDbContext.Create();

        public ApplicationDbContext Context { get; private set; }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
                Context = null;
            }
        }
        ~ContextProvider() => Dispose();
    }
}
