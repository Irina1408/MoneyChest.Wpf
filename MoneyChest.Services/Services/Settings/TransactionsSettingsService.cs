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
        public TransactionsSettingsService(ApplicationDbContext context) : base(context)
        {
        }

        protected override IQueryable<TransactionsSettings> Scope => Entities.Include(_ => _.PeriodFilter).Include(_ => _.DataFilter.Categories).Include(_ => _.DataFilter.Storages);

        //protected override TransactionsSettings Update(TransactionsSettings entity, TransactionsSettingsModel model)
        //{
        //    entity.DataFilter.Categories.Clear();
        //    SaveChanges();

        //    var categories = _context.Categories.Where(e => model.DataFilter.CategoryIds.Contains(e.Id)).ToList();
        //    categories.ForEach(e => entity.DataFilter.Categories.Add(e));

        //    return entity;
        //}
    }
}
