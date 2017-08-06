using MoneyChest.Data.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Mock
{
    public class DbFactory
    {
        private readonly ApplicationDbContext _db;
        private readonly Dictionary<Type, Action<object>> _defaults = new Dictionary<Type, Action<object>>();

        public DbFactory(ApplicationDbContext db)
        {
            _db = db;
            DbFactoryDefinitions.Define(this);
        }

        internal void Define<T>(Action<T> setters)
        {
            _defaults[typeof(T)] = (obj) => setters((T)obj);
        }

        public T Create<T>(Action<T> overrides = null) where T : class, new()
        {
            var r = new T();
            try
            {
                Action<object> defaultSetter;
                if (_defaults.TryGetValue(typeof(T), out defaultSetter))
                    defaultSetter.Invoke(r);

                overrides?.Invoke(r);

                _db.Entry(r).State = System.Data.Entity.EntityState.Added;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return r;
        }

        public T Get<T>(Expression<Func<T, bool>> lookup) where T : class
        {
            return _db.Set<T>().FirstOrDefault(lookup);
        }

        public List<T> GetAll<T>(Expression<Func<T, bool>> lookup) where T : class
        {
            return _db.Set<T>().Where(lookup).ToList();
        }

        public List<T> GetAllInclude<T, TInclude>(Expression<Func<T, bool>> lookup, Expression<Func<T, TInclude>> path) where T : class
        {
            return _db.Set<T>().Where(lookup).Include(path).ToList();
        }

        public T GetOrCreate<T>(Expression<Func<T, bool>> lookup, Action<T> overrides = null) where T : class, new()
        {
            var result = _db.Set<T>().FirstOrDefault(lookup);
            if (result == null)
                result = Create<T>(overrides);
            return result;
        }
    }
}
