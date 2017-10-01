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

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class CurrencyServiceTests : IdManageableUserableHistoricizedServiceTestBase<Currency, CurrencyService, CurrencyHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(Currency entity) => entity.Name = "Some other name";
        protected override void SetUserId(Currency entity, int userId) => entity.UserId = userId;

        #endregion

        [TestMethod]
        public virtual void ItSetsMainCurrency()
        {
            var currencyService = (CurrencyService)serviceIdManageable;

            // create entity
            var entity = App.Factory.Create<Currency>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.IsUsed = true;
            });
            currencyService.SetMain(user.Id, entity.Id);

            // check entity is main currency
            var currencies = currencyService.GetAllForUser(user.Id, _ => _.IsMain);
            currencies.Count.ShouldBeEquivalentTo(1);
            CheckAreEquivalent(currencies[0], entity);
        }

        [TestMethod]
        public virtual void ItFecthesMainCurrency()
        {
            var currencyService = (CurrencyService)serviceIdManageable;

            // create entity
            var entity = App.Factory.Create<Currency>(item => 
            {
                OnCreateOverrides?.Invoke(item);
                item.IsUsed = true;
            });
            currencyService.SetMain(user.Id, entity.Id);

            // check entity fetched
            var entityFetched = ((CurrencyService)serviceIdManageable).GetMain(user.Id);
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, entity);
        }
    }
}
