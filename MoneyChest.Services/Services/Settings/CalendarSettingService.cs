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
    public interface ICalendarSettingService : IBaseService<CalendarSetting>
    {
    }

    public class CalendarSettingService : BaseService<CalendarSetting>, ICalendarSettingService
    {
        public CalendarSettingService(ApplicationDbContext context) : base(context)
        {
        }
    }
}
