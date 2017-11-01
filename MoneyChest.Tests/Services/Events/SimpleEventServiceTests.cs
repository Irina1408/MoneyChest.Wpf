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
using MoneyChest.Model.Model;
using MoneyChest.Data.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services.Events
{
    [TestClass]
    public class SimpleEventServiceTests : HistoricizedIdManageableUserableListServiceTestBase<SimpleEvent, SimpleEventModel, SimpleEventConverter, SimpleEventService, SimpleEventHistory>
    {
        #region Overrides 

        protected override IQueryable<SimpleEvent> Scope => Entities.Include(_ => _.Storage).Include(_ => _.Currency).Include(_ => _.Category);
        protected override void ChangeEntity(SimpleEventModel entity) => entity.Description = "Some other description";
        protected override void SetUserId(SimpleEvent entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var category = App.Factory.Create<Category>(item => item.UserId = userId);
            entity.StorageId = storage.Id;
            entity.CurrencyId = storage.CurrencyId;
            entity.CategoryId = category.Id;
            entity.UserId = userId;
        }
        protected override void SetUserId(SimpleEventModel entity, int userId)
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
