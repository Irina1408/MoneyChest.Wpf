using MoneyChest.Data.Context;
using MoneyChest.Data.Mock;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Base
{
    public class ApplicationFixture : IDisposable
    {
        private readonly DbContextTransaction _transaction;

        public ApplicationFixture()
        {
            Db = new ApplicationDbContext(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString);
            Factory = new DbFactory(Db);
            _transaction = Db.Database.BeginTransaction();
        }

        public ApplicationDbContext Db { get; }

        public DbFactory Factory { get; private set; }

        public void Dispose()
        {
            _transaction.Rollback();
        }
    }
}
