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
    public class StorageServiceTests : HistoricizedIdManageableUserableListServiceTestBase<Storage, StorageModel, StorageConverter, StorageService, StorageHistory>
    {
        #region Overrides 

        protected override IQueryable<Storage> Scope => Entities.Include(_ => _.Currency).Include(_ => _.StorageGroup);
        protected override void ChangeEntity(StorageModel entity) => entity.Name = "Some other name";
        protected override void SetUserId(Storage entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            var storageGroup = App.Factory.Create<StorageGroup>(item => item.UserId = userId);

            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
            entity.StorageGroupId = storageGroup.Id;
        }
        protected override void SetUserId(StorageModel entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            var storageGroup = App.Factory.Create<StorageGroup>(item => item.UserId = userId);

            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
            entity.StorageGroupId = storageGroup.Id;
        }

        #endregion
    }
}
