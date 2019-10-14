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
using MoneyChest.Services.Utils;

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
            // update category list
            if (ServiceHelper.UpdateRelatedEntities(_context, entity.Categories, model.CategoryIds))
                SaveChanges();

            return entity;
        }
    }
}
