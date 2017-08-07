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
using MoneyChest.Data.Mock;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class DebtServiceTests : IdManageableUserableHistoricizedServiceTestBase<Debt, DebtService, DebtHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(Debt entity) => entity.Name = "Some other name";
        protected override void SetUserId(Debt entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            var storage = App.Factory.CreateStorage(userId);
            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
            entity.StorageId = storage.Id;
        }

        #endregion
    }
}
