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
using MoneyChest.Data.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class MoneyTransferServiceTests : HistoricizedIdManageableServiceTestBase<MoneyTransfer, MoneyTransferModel, MoneyTransferConverter, MoneyTransferService, MoneyTransferHistory>
    {
        #region Overrides 

        protected override IQueryable<MoneyTransfer> Scope => Entities.Include(_ => _.StorageFrom.Currency).Include(_ => _.StorageTo.Currency).Include(_ => _.Category);
        protected override void ChangeEntity(MoneyTransferModel entity) => entity.Value += 100;
        protected override void SetUserId(MoneyTransfer entity, int userId)
        {
            var storage1 = App.Factory.CreateStorage(userId);
            var storage2 = App.Factory.CreateStorage(userId);

            entity.StorageFrom = storage1;
            entity.StorageTo = storage2;
            entity.StorageFromId = storage1.Id;
            entity.StorageToId = storage2.Id;
        }
        protected override void SetUserId(MoneyTransferModel entity, int userId)
        {
            var storage1 = App.Factory.CreateStorage(userId);
            var storage2 = App.Factory.CreateStorage(userId);
            
            entity.StorageFromId = storage1.Id;
            entity.StorageToId = storage2.Id;
        }

        #endregion
    }
}
