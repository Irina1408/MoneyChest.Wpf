using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IIdManagableServiceBase<T> : IServiceBase<T>, IIdManageable<T>, IIdListManageable<T>
        where T : class
    {
    }
}
