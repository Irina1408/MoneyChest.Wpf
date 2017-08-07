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

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class LimitServiceTests : IdManageableUserableHistoricizedServiceTestBase<Limit, LimitService, LimitHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(Limit entity) => entity.Value += 150;
        protected override void SetUserId(Limit entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
        }

        #endregion
    }
}
