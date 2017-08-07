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
using MoneyChest.Services.Services.Events;
using MoneyChest.Data.Mock;

namespace MoneyChest.Tests.Services.Events
{
    [TestClass]
    public class RepayDebtEventServiceTests : IdManageableUserableHistoricizedServiceTestBase<RepayDebtEvent, RepayDebtEventService, RepayDebtEventHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(RepayDebtEvent entity) => entity.Description = "Some other description";
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

        #endregion
    }
}
