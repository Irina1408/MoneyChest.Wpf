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
    }
}
