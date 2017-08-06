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
    public interface IGeneralSettingService : IBaseUserableService<GeneralSetting>
    {
    }

    public class GeneralSettingService : BaseUserableService<GeneralSetting>, IGeneralSettingService
    {
        public GeneralSettingService(ApplicationDbContext context) : base(context)
        {
        }

        public override Func<GeneralSetting, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
