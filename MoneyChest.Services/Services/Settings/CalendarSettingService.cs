using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services
{
    public interface ICalendarSettingService : IBaseUserableService<CalendarSetting>
    {
    }

    public class CalendarSettingService : BaseUserableService<CalendarSetting>, ICalendarSettingService
    {
        public CalendarSettingService(ApplicationDbContext context) : base(context)
        {
        }

        public override Func<CalendarSetting, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
