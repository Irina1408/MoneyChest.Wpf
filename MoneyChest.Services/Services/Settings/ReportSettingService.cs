using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using System.Data.Entity;

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

        public override ReportSetting GetForUser(int userId, Expression<Func<ReportSetting, bool>> expression = null)
        {
            if (expression == null) expression = item => true;
            return Entities.Include(_ => _.Categories).Where(LimitByUser(userId)).FirstOrDefault(expression);
        }

        protected override int UserId(ReportSetting entity) => entity.UserId;

        protected override Expression<Func<ReportSetting, bool>> LimitByUser(int userId) => item => item.UserId == userId;
    }
}
