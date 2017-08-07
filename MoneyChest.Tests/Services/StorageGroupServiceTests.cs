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
    public class StorageGroupServiceTests : IdManageableUserableHistoricizedServiceTestBase<StorageGroup, StorageGroupService, StorageGroupHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(StorageGroup entity) => entity.Name = "Some other name";
        protected override void SetUserId(StorageGroup entity, int userId) => entity.UserId = userId;

        #endregion
    }
}
