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
using MoneyChest.Model.Model;
using MoneyChest.Data.Converters;

namespace MoneyChest.Services.Services.Settings
{
    public interface IReportSettingService : IUserSettingsService<ReportSettingModel>
    {
    }

    public class ReportSettingService : BaseUserSettingService<ReportSetting, ReportSettingModel, ReportSettingConverter>, IReportSettingService
    {
        public ReportSettingService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<ReportSetting> Scope => Entities.Include(_ => _.Categories);

        protected override ReportSetting Update(ReportSetting entity, ReportSettingModel model)
        {
            entity.Categories.Clear();
            SaveChanges();

            var categories = _context.Categories.Where(e => model.CategoryIds.Contains(e.Id)).ToList();
            categories.ForEach(e => entity.Categories.Add(e));

            return entity;
        }
    }
}
