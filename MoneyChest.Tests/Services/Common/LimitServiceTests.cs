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
    public class LimitServiceTests : HistoricizedIdManageableUserableListServiceTestBase<Limit, LimitModel, LimitConverter, LimitService, LimitHistory>
    {
        #region Overrides 

        protected override IQueryable<Limit> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category);
        protected override void ChangeEntity(LimitModel entity) => entity.Value += 150;
        protected override void SetUserId(Limit entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
        }
        protected override void SetUserId(LimitModel entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
        }

        #endregion
    }
}
