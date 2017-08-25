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
using MoneyChest.Data.Enums;
using System.Linq.Expressions;
using System.Data.Entity;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class UserServiceTests : IdManageableUserableHistoricizedServiceTestBase<User, UserService, UserHistory>
    {
        #region Defaults

        [TestMethod]
        public void ItLoadsDefaultEngCategoriesForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.English);

            // check entities fetched
            var userDb = service.GetForUser(user.Id);
            userDb.Should().NotBeNull();

            // check categories
            var categories = App.Db.Categories.Where(item => item.UserId == user.Id).ToList();
            (categories.Count > 0).ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        public void ItLoadsDefaultEngCurrenciesForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.English);

            // check entities fetched
            var userDb = service.GetForUser(user.Id);
            userDb.Should().NotBeNull();

            // check Currencies
            var currencies = App.Db.Currencies.Where(item => item.UserId == user.Id).ToList();
            (currencies.Count > 0).ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        public void ItLoadsDefaultEngStoragesForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.English);

            // check entities fetched
            var userDb = service.GetForUser(user.Id);
            userDb.Should().NotBeNull();

            // check StorageGroups
            var storageGroups = App.Db.StorageGroups.Where(item => item.UserId == user.Id).ToList();
            (storageGroups.Count > 0).ShouldBeEquivalentTo(true);

            // check Storages
            var storages = App.Db.Storages.Where(item => item.UserId == user.Id).ToList();
            (storages.Count > 0).ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        public void ItLoadsDefaultEngSettingsForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.English);

            // check user and settings
            var userDb = App.Db.Users
                .Include(_ => _.ForecastSettings)
                .Include(_ => _.CalendarSettings)
                .Include(_ => _.GeneralSettings)
                .Include(_ => _.ReportSettings)
                .Include(_ => _.RecordsViewFilter)
                .Where(item => item.Id == user.Id)
                .FirstOrDefault();

            userDb.Should().NotBeNull();
            userDb.ForecastSettings.Should().NotBeNull();
            userDb.CalendarSettings.Should().NotBeNull();
            userDb.GeneralSettings.Should().NotBeNull();
            userDb.ReportSettings.Should().NotBeNull();
            userDb.RecordsViewFilter.Should().NotBeNull();
        }

        [TestMethod]
        public void ItLoadsDefaultRusCategoriesForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.Russian);

            // check entities fetched
            var userDb = service.GetForUser(user.Id);
            userDb.Should().NotBeNull();

            // check categories
            var categories = App.Db.Categories.Where(item => item.UserId == user.Id).ToList();
            (categories.Count > 0).ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        public void ItLoadsDefaultRusCurrenciesForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.Russian);

            // check entities fetched
            var userDb = service.GetForUser(user.Id);
            userDb.Should().NotBeNull();

            // check Currencies
            var currencies = App.Db.Currencies.Where(item => item.UserId == user.Id).ToList();
            (currencies.Count > 0).ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        public void ItLoadsDefaultRusStoragesForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.Russian);

            // check entities fetched
            var userDb = service.GetForUser(user.Id);
            userDb.Should().NotBeNull();

            // check StorageGroups
            var storageGroups = App.Db.StorageGroups.Where(item => item.UserId == user.Id).ToList();
            (storageGroups.Count > 0).ShouldBeEquivalentTo(true);

            // check Storages
            var storages = App.Db.Storages.Where(item => item.UserId == user.Id).ToList();
            (storages.Count > 0).ShouldBeEquivalentTo(true);
        }

        [TestMethod]
        public void ItLoadsDefaultRusSettingsForUser()
        {
            // create user
            var user = App.Factory.CreateInstance<User>(OnCreateOverrides);
            ((UserService)service).Add(user, Language.Russian);

            // check user and settings
            var userDb = App.Db.Users
                .Include(_ => _.ForecastSettings)
                .Include(_ => _.CalendarSettings)
                .Include(_ => _.GeneralSettings)
                .Include(_ => _.ReportSettings)
                .Include(_ => _.RecordsViewFilter)
                .Where(item => item.Id == user.Id)
                .FirstOrDefault();

            userDb.Should().NotBeNull();
            userDb.ForecastSettings.Should().NotBeNull();
            userDb.CalendarSettings.Should().NotBeNull();
            userDb.GeneralSettings.Should().NotBeNull();
            userDb.ReportSettings.Should().NotBeNull();
            userDb.RecordsViewFilter.Should().NotBeNull();
        }

        #endregion

        #region Overrides 

        [TestMethod]
        public override void ItFetchesAllEntitiesForUser()
        {
            // create entities
            var entity1 = App.Factory.Create<User>(OnCreateOverrides);

            // check entities fetched
            var entities = service.GetAllForUser(entity1.Id);
            entities.Count.ShouldBeEquivalentTo(1);
        }

        [TestMethod]
        public override void ItRemovesEntityById()
        {
            // not check user deletion
        }

        [TestMethod]
        public override void ItRemovesEntity()
        {
            // not check user deletion
        }

        protected override void ChangeEntity(User entity) => entity.Name = "Some other name";
        protected override void SetUserId(User entity, int userId) { }
        protected override int GetUserId(User entity) => entity.Id;
        protected override bool CreateUserSettings => false;

        #endregion
    }
}
