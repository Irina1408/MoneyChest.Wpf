using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Settings
{
    public interface IGeneralSettingService : IBaseService<GeneralSetting>
    {
    }

    public class GeneralSettingService : BaseService<GeneralSetting>, IGeneralSettingService
    {
        public GeneralSettingService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
