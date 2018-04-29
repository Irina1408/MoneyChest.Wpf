using MoneyChest.Data.Attributes;
using MoneyChest.Data.Context;
using MoneyChest.Data.Enums;
using MoneyChest.Data.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public abstract class ServiceBase : IServiceBase
    {
        protected ApplicationDbContext _context;

        public ServiceBase(ApplicationDbContext context)
        {
            _context = context;
        }

        #region IBaseService implementation

        public virtual void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                // cleanup changes
                ReloadChanged();

                // wrap common exceptions
                var sqlException = dbUpdateException.InnerException.InnerException as SqlException;

                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 515: throw new ViolationOfConstraintException("A constraint violation of null value has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 547: throw new ReferenceConstraintException("A constraint reference has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 2627: throw new ViolationOfConstraintException("A constraint violation has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        default: throw;
                    }
                }

                throw;
            }
            catch(Exception)
            {
                // cleanup changes
                ReloadChanged();

                throw;
            }
        }

        public virtual async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                // cleanup changes
                await ReloadChangedAsync();

                // wrap common exceptions
                var sqlException = dbUpdateException.InnerException.InnerException as SqlException;

                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 515: throw new ViolationOfConstraintException("A constraint violation of null value has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 547: throw new ReferenceConstraintException("A constraint reference has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 2627: throw new ViolationOfConstraintException("A constraint violation has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        default: throw;
                    }
                }

                throw;
            }
            catch (Exception)
            {
                // cleanup changes
                await ReloadChangedAsync();

                throw;
            }
        }

        private void ReloadChanged() =>
            _context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList().ForEach(x => x.Reload());

        private async Task ReloadChangedAsync()
        {
            foreach(var x in _context.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList())
                await x.ReloadAsync();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            if(_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        #endregion
    }
}
