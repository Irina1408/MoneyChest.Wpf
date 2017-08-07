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

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class RecordServiceTests : IdManageableUserableHistoricizedServiceTestBase<Record, RecordService, RecordHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(Record entity) => entity.Description = "Some other Description";
        protected override void SetUserId(Record entity, int userId)
        {
            var currency = App.Factory.Create<Currency>(item => item.UserId = userId);
            var category = App.Factory.Create<Category>(item => item.UserId = userId);
            var storage = App.Factory.CreateStorage(userId);
            entity.UserId = userId;
            entity.CurrencyId = currency.Id;
            entity.StorageId = storage.Id;
            entity.CategoryId = category.Id;
        }

        #endregion
    }
}
