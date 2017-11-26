using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class CurrencyServiceTests : HistoricizedIdManageableUserableListServiceTestBase<Currency, CurrencyModel, CurrencyConverter, CurrencyService, CurrencyHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(CurrencyModel entity) => entity.Name = "Some other name";

        #endregion

        [TestMethod]
        public virtual void ItSetsMainCurrency()
        {
            var currencyService = (CurrencyService)serviceIdManageable;

            // create entity
            var entity = App.Factory.Create<Currency>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.IsActive = true;
            });

            currencyService.SetMain(user.Id, entity.Id);

            // check entity is main currency
            var currencies = currencyService.GetListForUser(user.Id).Where( _ => _.IsMain).ToList();
            currencies.Count.ShouldBeEquivalentTo(1);
            CheckAreEquivalent(currencies[0], converter.ToModel(entity));
        }

        [TestMethod]
        public virtual void ItFecthesMainCurrency()
        {
            var currencyService = (CurrencyService)serviceIdManageable;

            // create entity
            var entity = App.Factory.Create<Currency>(item => 
            {
                OnCreateOverrides?.Invoke(item);
                item.IsActive = true;
            });
            currencyService.SetMain(user.Id, entity.Id);

            // check entity fetched
            var entityFetched = ((CurrencyService)serviceIdManageable).GetMain(user.Id);
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, converter.ToModel(entity));
        }
    }
}
