using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;

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

        protected override int UserId(GeneralSetting entity) => entity.UserId;

        protected override Expression<Func<GeneralSetting, bool>> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
