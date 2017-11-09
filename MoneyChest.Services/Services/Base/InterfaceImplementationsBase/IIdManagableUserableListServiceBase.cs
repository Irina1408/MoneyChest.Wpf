using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IIdManagableUserableListServiceBase<T> : IIdManagableServiceBase<T>, IUserableListService<T>
        where T : class
    {
    }
}
