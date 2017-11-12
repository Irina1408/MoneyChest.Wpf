using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Context;
using MoneyChest.Data.Entities;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services
{
    public interface IForecastSettingService : IUserSettingsService<ForecastSettingModel>
    {
    }

    public class ForecastSettingService : UserSettingServiceBase<ForecastSetting, ForecastSettingModel, ForecastSettingConverter>, IForecastSettingService
    {
        public ForecastSettingService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<ForecastSetting> Scope => Entities.Include(_ => _.Categories);

        protected override ForecastSetting Update(ForecastSetting entity, ForecastSettingModel model)
        {
            entity.Categories.Clear();
            SaveChanges();

            var categories = _context.Categories.Where(e => model.CategoryIds.Contains(e.Id)).ToList();
            categories.ForEach(e => entity.Categories.Add(e));

            return entity;
        }
    }
}
