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

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class StorageGroupServiceTests : HistoricizedIdManageableUserableListServiceTestBase<StorageGroup, StorageGroupModel, StorageGroupConverter, StorageGroupService, StorageGroupHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(StorageGroupModel entity) => entity.Name = "Some other name";

        #endregion
    }
}
