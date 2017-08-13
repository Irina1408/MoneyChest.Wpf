using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Services.Services.Base
{
    public interface IIdManageable<T>
    {
        T Get(int id);
        List<T> Get(List<int> ids);
        void Delete(int id);
    }
}
