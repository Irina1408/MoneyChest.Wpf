using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Calculation.Builders.Base
{
    public interface IDataBuilder<TSettings, TResultItem>
    {
        List<TResultItem> Build(TSettings settings);
    }
}
