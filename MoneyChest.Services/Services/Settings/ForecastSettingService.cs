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
    public interface IForecastSettingService : IBaseUserableService<ForecastSetting>
    {
    }

    public class ForecastSettingService : BaseUserableService<ForecastSetting>, IForecastSettingService
    {
        public ForecastSettingService(ApplicationDbContext context) : base(context)
        {
        }

        protected override int UserId(ForecastSetting entity) => entity.UserId;

        protected override Func<ForecastSetting, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
