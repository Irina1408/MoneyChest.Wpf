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
    public class MoneyTransferEventServiceTests : IdManageableUserableHistoricizedServiceTestBase<MoneyTransferEvent, MoneyTransferEventService, MoneyTransferEventHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(MoneyTransferEvent entity) => entity.Description = "Some other description";
        protected override void SetUserId(MoneyTransferEvent entity, int userId)
        {
            var storage1 = App.Factory.CreateStorage(userId);
            var storage2 = App.Factory.CreateStorage(userId);

            entity.StorageFromId = storage1.Id;
            entity.StorageToId = storage2.Id;
            entity.UserId = userId;
        }

        #endregion
    }
}
