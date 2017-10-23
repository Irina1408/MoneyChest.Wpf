using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IBaseIdManagableUserableListService<T> : IBaseIdManagableService<T>, IUserableListService<T>
        where T : class
    {
    }
}
