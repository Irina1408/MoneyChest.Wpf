using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Entities.History;
using MoneyChest.Services.Services;
using MoneyChest.Model.Model;
using MoneyChest.Data.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class CurrencyExchangeRateServiceTests 
        : HistoricizedEntityModelServiceTestBase<CurrencyExchangeRate, CurrencyExchangeRateModel, CurrencyExchangeRateConverter, CurrencyExchangeRateService, CurrencyExchangeRateHistory>
    {
        protected CurrencyExchangeRateService currService;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            currService = (CurrencyExchangeRateService)service;
        }

        #region Overrides 

        protected override IQueryable<CurrencyExchangeRate> Scope => Entities.Include(_ => _.CurrencyFrom).Include(_ => _.CurrencyTo);

        protected override void ChangeEntity(CurrencyExchangeRateModel entity) => entity.Rate += 5;

        protected override CurrencyExchangeRateModel FetchItem(CurrencyExchangeRateModel model) => currService.GetListForUser(user.Id).FirstOrDefault(e => e.CurrencyFromId == model.CurrencyFromId && e.CurrencyToId == model.CurrencyToId);

        protected override void SetUserId(CurrencyExchangeRate entity, int userId)
        {
            var currency1 = App.Factory.Create<Currency>(item => item.UserId = userId);
            var currency2 = App.Factory.Create<Currency>(item => item.UserId = userId);

            entity.CurrencyFromId = currency1.Id;
            entity.CurrencyToId = currency2.Id;
        }

        protected override void SetUserId(CurrencyExchangeRateModel entity, int userId)
        {
            var currency1 = App.Factory.Create<Currency>(item => item.UserId = userId);
            var currency2 = App.Factory.Create<Currency>(item => item.UserId = userId);

            entity.CurrencyFromId = currency1.Id;
            entity.CurrencyToId = currency2.Id;
        }

        protected override CurrencyExchangeRate GetDbItem(CurrencyExchangeRateModel model) => Scope.FirstOrDefault(e => e.CurrencyFromId == model.CurrencyFromId && e.CurrencyToId == model.CurrencyToId);
        protected override CurrencyExchangeRate GetDbItem(CurrencyExchangeRate entity) => Scope.FirstOrDefault(e => e.CurrencyFromId == entity.CurrencyFromId && e.CurrencyToId == entity.CurrencyToId);
        
        #endregion
    }
}
