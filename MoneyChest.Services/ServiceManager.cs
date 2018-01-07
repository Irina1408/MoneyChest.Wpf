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
        private static ApplicationDbContext _context;
        
        public static TService ConfigureService<TService>()
            where TService : ServiceBase
        {
            Initialize();
            return Activator.CreateInstance(typeof(TService), _context) as TService;
        }

        public static void Initialize()
        {
            if (_context == null)
                _context = ApplicationDbContext.Create();
        }

        public static void Dispose()
        {
            if(_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }
    }
}
