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
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services.Events
{
    [TestClass]
    public class RepayDebtEventServiceTests : HistoricizedIdManageableUserableListServiceTestBase<RepayDebtEvent, RepayDebtEventModel, RepayDebtEventConverter, RepayDebtEventService, RepayDebtEventHistory>
    {
        #region Overrides 

        protected override IQueryable<RepayDebtEvent> Scope => Entities.Include(_ => _.Storage.Currency).Include(_ => _.Debt.Currency);
        protected override void ChangeEntity(RepayDebtEventModel entity) => entity.Description = "Some other description";
        protected override void SetUserId(RepayDebtEvent entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var debt = App.Factory.Create<Debt>(item => 
            {
                item.UserId = userId;
                item.CurrencyId = storage.CurrencyId;
                item.StorageId = storage.Id;
            });
            entity.StorageId = storage.Id;
            entity.DebtId = debt.Id;
            entity.UserId = userId;
        }
        protected override void SetUserId(RepayDebtEventModel entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var debt = App.Factory.Create<Debt>(item =>
            {
                item.UserId = userId;
                item.CurrencyId = storage.CurrencyId;
                item.StorageId = storage.Id;
            });
            entity.StorageId = storage.Id;
            entity.DebtId = debt.Id;
            entity.UserId = userId;
        }

        #endregion
    }
}
