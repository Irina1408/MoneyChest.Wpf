using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IUserableService<T>
    {
        T GetForUser(int userId);
    }

    public interface IUserableListService<T>
    {
        List<T> GetListForUser(int userId);
    }
}
