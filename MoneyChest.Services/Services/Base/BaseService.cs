using MoneyChest.Data.Attributes;
using MoneyChest.Data.Context;
using MoneyChest.Data.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public abstract class BaseService : IBaseService
    {
        protected ApplicationDbContext _context;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region IBaseService implementation

        public virtual void SaveChanges()
        {
            _context.SaveChanges();
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}
