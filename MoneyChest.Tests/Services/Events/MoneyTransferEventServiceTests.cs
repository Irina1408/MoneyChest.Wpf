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
    public class MoneyTransferEventServiceTests : HistoricizedIdManageableUserableListServiceTestBase<MoneyTransferEvent, MoneyTransferEventModel, MoneyTransferEventConverter, MoneyTransferEventService, MoneyTransferEventHistory>
    {
        #region Overrides 

        protected override IQueryable<MoneyTransferEvent> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);
        protected override void ChangeEntity(MoneyTransferEventModel entity) => entity.Description = "Some other description";
        protected override void SetUserId(MoneyTransferEvent entity, int userId)
        {
            var storage1 = App.Factory.CreateStorage(userId);
            var storage2 = App.Factory.CreateStorage(userId);

            entity.StorageFromId = storage1.Id;
            entity.StorageToId = storage2.Id;
            entity.UserId = userId;
        }
        protected override void SetUserId(MoneyTransferEventModel entity, int userId)
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
