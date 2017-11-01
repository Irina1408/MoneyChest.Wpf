using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.View.Pages
{
    public interface IPage
    {
        string Name { get; }
        object Icon { get; }
    }
}
