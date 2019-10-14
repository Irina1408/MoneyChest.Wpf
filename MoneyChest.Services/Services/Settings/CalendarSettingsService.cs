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
using MoneyChest.Services.Converters;
using System.Data.Entity;
using MoneyChest.Services.Utils;

namespace MoneyChest.Services.Services
{
    public interface ICalendarSettingsService : IUserSettingsService<CalendarSettingsModel>
    {
    }

    public class CalendarSettingsService : UserSettingServiceBase<CalendarSettings, CalendarSettingsModel, CalendarSettingsConverter>, ICalendarSettingsService
    {
        private DataFilterConverter dataFilterConverter = new DataFilterConverter();
        private PeriodFilterConverter periodFilterConverter = new PeriodFilterConverter();

        public CalendarSettingsService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<CalendarSettings> Scope => Entities.Include(_ => _.PeriodFilter).Include(_ => _.DataFilter.Categories).Include(_ => _.DataFilter.Storages);

        public override CalendarSettingsModel GetForUser(int userId)
        {
            var settings = Scope.FirstOrDefault(e => e.UserId == userId);
            if (settings == null)
            {
                // add datafilter (use convertion to apply defaults model)
                var dataFilter = dataFilterConverter.ToEntity(new DataFilterModel());
                _context.DataFilters.Add(dataFilter);
                // add period filter (use convertion to apply defaults model)
                var periodFilter = periodFilterConverter.ToEntity(new PeriodFilterModel());
                _context.PeriodFilters.Add(periodFilter);
                // add full filter
                Entities.Add(new CalendarSettings() { UserId = userId, ShowAllLimits = true, DataFilter = dataFilter, PeriodFilter = periodFilter });
                _context.SaveChanges();

                settings = Scope.FirstOrDefault(e => e.UserId == userId);
            }

            return _converter.ToModel(settings);
        }

        protected override CalendarSettings Update(CalendarSettings entity, CalendarSettingsModel model)
        {
            // update categories and storages lists
            if (ServiceHelper.UpdateRelatedEntities(_context, entity.DataFilter.Categories, model.DataFilter.CategoryIds)
                || ServiceHelper.UpdateRelatedEntities(_context, entity.DataFilter.Storages, model.DataFilter.StorageIds))
            {
                SaveChanges();
            }

            return entity;
        }
    }
}
