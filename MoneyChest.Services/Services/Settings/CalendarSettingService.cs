using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services
{
    public interface ICalendarSettingService : IUserSettingsService<CalendarSettingModel>
    {
    }

    public class CalendarSettingService : BaseUserSettingService<CalendarSetting, CalendarSettingModel, CalendarSettingConverter>, ICalendarSettingService
    {
        public CalendarSettingService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<CalendarSetting> Scope => Entities.Include(_ => _.StorageGroups);

        protected override CalendarSetting Update(CalendarSetting entity, CalendarSettingModel model)
        {
            entity.StorageGroups.Clear();
            SaveChanges();

            var storageGroups = _context.StorageGroups.Where(e => model.StorageGroupIds.Contains(e.Id)).ToList();
            storageGroups.ForEach(e => entity.StorageGroups.Add(e));

            return entity;
        }
    }
}
