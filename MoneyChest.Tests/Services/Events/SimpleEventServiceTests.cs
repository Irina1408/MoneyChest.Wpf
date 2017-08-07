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
    public class SimpleEventServiceTests : IdManageableUserableHistoricizedServiceTestBase<SimpleEvent, SimpleEventService, SimpleEventHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(SimpleEvent entity) => entity.Description = "Some other description";
        protected override void SetUserId(SimpleEvent entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var category = App.Factory.Create<Category>(item => item.UserId = userId);
            entity.StorageId = storage.Id;
            entity.CurrencyId = storage.CurrencyId;
            entity.CategoryId = category.Id;
            entity.UserId = userId;
        }

        #endregion
    }
}
