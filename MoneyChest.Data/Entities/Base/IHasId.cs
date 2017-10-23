using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.Base
{
    public interface IHasId
    {
        int Id { get; set; }
    }
}
