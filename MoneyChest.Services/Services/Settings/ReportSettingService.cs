using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;

namespace MoneyChest.Services.Services.Settings
{
    public interface IReportSettingService : IBaseUserableService<ReportSetting>
    {
    }

    public class ReportSettingService : BaseUserableService<ReportSetting>, IReportSettingService
    {
        public ReportSettingService(ApplicationDbContext context) : base(context)
        {
        }

        public override Func<ReportSetting, bool> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
