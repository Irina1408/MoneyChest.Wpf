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
        void Delete(int id);
    }
}
