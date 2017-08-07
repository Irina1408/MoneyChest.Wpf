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
    public class StorageServiceTests : IdManageableUserableHistoricizedServiceTestBase<Storage, StorageService, StorageHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(Storage entity) => entity.Name = "Some other name";
        protected override void SetUserId(Storage entity, int userId)
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
