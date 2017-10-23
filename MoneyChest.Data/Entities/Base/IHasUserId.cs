using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Data.Entities.Base
{
    public interface IHasUserId
    {
        int UserId { get; set; }
    }
}
