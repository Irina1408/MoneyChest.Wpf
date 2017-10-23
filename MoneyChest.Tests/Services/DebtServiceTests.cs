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
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class DebtServiceTests : HistoricizedIdManageableUserableListServiceTestBase<Debt, DebtModel, DebtConverter, DebtService, DebtHistory>
    {
        #region Overrides 

        protected override IQueryable<Debt> Scope => Entities.Include(_ => _.Currency);
        protected override void ChangeEntity(DebtModel entity) => entity.Name = "Some other name";
        protected override void SetUserId(Debt entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            var storage = App.Factory.CreateStorage(userId);
            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
            entity.StorageId = storage.Id;
        }
        protected override void SetUserId(DebtModel entity, int userId)
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
