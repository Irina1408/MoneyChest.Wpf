using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;

namespace MoneyChest.Services.Services
{
    public class ForecastSettingService : BaseService<ForecastSetting>
    {
        public ForecastSettingService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
