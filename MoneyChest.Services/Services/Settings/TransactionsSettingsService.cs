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

namespace MoneyChest.Services.Services
{
    public interface ITransactionsSettingsService : IUserSettingsService<TransactionsSettingsModel>
    {
    }

    public class TransactionsSettingsService : UserSettingServiceBase<TransactionsSettings, TransactionsSettingsModel, TransactionsSettingsConverter>, ITransactionsSettingsService
    {
        private DataFilterConverter dataFilterConverter = new DataFilterConverter();
        private PeriodFilterConverter periodFilterConverter = new PeriodFilterConverter();

        public TransactionsSettingsService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<TransactionsSettings> Scope => Entities.Include(_ => _.PeriodFilter).Include(_ => _.DataFilter.Categories).Include(_ => _.DataFilter.Storages);

        public override TransactionsSettingsModel GetForUser(int userId)
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
                Entities.Add(new TransactionsSettings() { UserId = userId, DataFilter = dataFilter, PeriodFilter = periodFilter });
                _context.SaveChanges();

                settings = Scope.FirstOrDefault(e => e.UserId == userId);
            }

            return _converter.ToModel(settings);
        }

        protected override TransactionsSettings Update(TransactionsSettings entity, TransactionsSettingsModel model)
        {
            entity.DataFilter.Categories.Clear();
            entity.DataFilter.Storages.Clear();
            SaveChanges();

            var categories = _context.Categories.Where(e => model.DataFilter.CategoryIds.Contains(e.Id)).ToList();
            categories.ForEach(e => entity.DataFilter.Categories.Add(e));

            var storages = _context.Storages.Where(e => model.DataFilter.StorageIds.Contains(e.Id)).ToList();
            storages.ForEach(e => entity.DataFilter.Storages.Add(e));

            return entity;
        }
    }
}
