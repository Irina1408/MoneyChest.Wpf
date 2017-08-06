﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Enums;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests : HistoricizedServiceTestBase
    {
        [TestMethod]
        public void ItFetchesCategoriesForUser()
        {
            var service = new CategoryService(App.Db);
            var category = App.Factory.Create<Category>(item => item.UserId = user.Id);

            // check category fetched
            var categories = service.GetAllForUser(user.Id);
            categories.Count.ShouldBeEquivalentTo(1);
            categories[0].Id.ShouldBeEquivalentTo(category.Id);
            categories[0].Name.ShouldBeEquivalentTo(category.Name);
        }

        [TestMethod]
        public void ItFetchesCategoryById()
        {
            var service = new CategoryService(App.Db);
            var category = App.Factory.Create<Category>(item => item.UserId = user.Id);

            // check category fetched
            var categories = service.Get(category.Id);
            categories.Should().NotBeNull();
        }

        [TestMethod]
        public void ItAddsCategory()
        {
            var service = new CategoryService(App.Db);
            var category = new Category()
            {
                Name = "SomeName",
                UserId = user.Id
            };
            service.Add(category);
            service.SaveChanges();

            // check category exists
            var categories = service.GetAllForUser(user.Id);
            categories.Count.ShouldBeEquivalentTo(1);
            categories[0].Id.ShouldBeEquivalentTo(category.Id);
            categories[0].Name.ShouldBeEquivalentTo(category.Name);
            CheckLastChangeHistory<Category, CategoryHistory>(ActionType.Add, category);
        }

        [TestMethod]
        public void ItSavesChanges()
        {
            var service = new CategoryService(App.Db);
            var category = App.Factory.Create<Category>(item => item.UserId = user.Id);
            var cat = service.GetForUser(user.Id, item => item.Id == category.Id);
            cat.Name = "Other name";
            service.SaveChanges();

            // check category changes saved
            var catSaved = service.GetForUser(user.Id);
            catSaved.Id.ShouldBeEquivalentTo(category.Id);
            catSaved.Name.ShouldBeEquivalentTo(cat.Name);
            CheckLastChangeHistory<Category, CategoryHistory>(ActionType.Update, category);
        }

        [TestMethod]
        public void ItRemovesCategory()
        {
            var service = new CategoryService(App.Db);
            var category = App.Factory.Create<Category>(item => item.UserId = user.Id);
            var cat = service.GetForUser(user.Id, item => item.Id == category.Id);
            service.Delete(cat);
            service.SaveChanges();

            // check category was removed
            var categories = service.GetAllForUser(user.Id);
            categories.Count.ShouldBeEquivalentTo(0);
            CheckLastChangeHistory<Category, CategoryHistory>(ActionType.Delete, cat);
        }

        [TestMethod]
        public void ItRemovesCategoryById()
        {
            var service = new CategoryService(App.Db);
            var category = App.Factory.Create<Category>(item => item.UserId = user.Id);
            var cat = service.GetForUser(user.Id, item => item.Id == category.Id);
            service.Delete(cat.Id);
            service.SaveChanges();

            // check category was removed
            var catRemoved= service.Get(cat.Id);
            catRemoved.Should().BeNull();
            CheckLastChangeHistory<Category, CategoryHistory>(ActionType.Delete, cat);
        }
    }
}