using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using MoneyChest.Data.Context;
using System.Linq.Expressions;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Services.Services.Settings
{
    public interface IRecordsViewFilterService : IUserSettingsService<RecordsViewFilterModel>
    {
    }

    public class RecordsViewFilterService : UserSettingServiceBase<RecordsViewFilter, RecordsViewFilterModel, RecordsViewFilterConverter>, IRecordsViewFilterService
    {
        public RecordsViewFilterService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<RecordsViewFilter> Scope => Entities.Include(_ => _.Categories);

        protected override RecordsViewFilter Update(RecordsViewFilter entity, RecordsViewFilterModel model)
        {
            entity.Categories.Clear();
            SaveChanges();

            var categories = _context.Categories.Where(e => model.CategoryIds.Contains(e.Id)).ToList();
            categories.ForEach(e => entity.Categories.Add(e));

            return entity;
        }
    }
}
