using MoneyChest.Data.Entities;
using MoneyChest.Data.Mock.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Mock
{
    public partial class DbFactoryDefinitions
    {
        public static void Define(DbFactory f)
        {
            f.Define<User>(e =>
            {
                e.Name = Moniker.UserName;
                e.Password = Moniker.UserPassword;
            });

            f.Define<Category>(e =>
            {
                e.Name = Moniker.Category;
            });
        }
    }
}
