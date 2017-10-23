using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Data.Entities.History;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests : HistoricizedIdManageableUserableListServiceTestBase<Category, CategoryModel, CategoryConverter, CategoryService, CategoryHistory>
    {
        [TestMethod]
        public virtual void ItFecthesLowestCategoryLevel()
        {
            // create entity
            var entity1 = App.Factory.Create<Category>(OnCreateOverrides);
            var entity2 = App.Factory.Create<Category>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.ParentCategoryId = entity1.Id;
            });

            // check entity fetched
            var entityFetched = ((CategoryService)serviceIdManageable).GetLowestCategoryLevel(user.Id);
            entityFetched.ShouldBeEquivalentTo(1);
        }

        [TestMethod]
        public virtual void ItFecthesCategoryMappingForLevel()
        {
            // create entity
            var entity1 = App.Factory.Create<Category>(OnCreateOverrides);
            var entity2 = App.Factory.Create<Category>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.ParentCategoryId = entity1.Id;
            });
            var entity3 = App.Factory.Create<Category>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.ParentCategoryId = entity2.Id;
            });

            // check entity fetched
            var mappingFetched = ((CategoryService)serviceIdManageable).GetCategoryMapping(user.Id, 1);
            mappingFetched.Should().ContainKey(entity1.Id);
            mappingFetched[entity1.Id].ShouldBeEquivalentTo(entity1.Id);
            mappingFetched.Should().ContainKey(entity2.Id);
            mappingFetched[entity2.Id].ShouldBeEquivalentTo(entity2.Id);
            mappingFetched.Should().ContainKey(entity3.Id);
            mappingFetched[entity3.Id].ShouldBeEquivalentTo(entity2.Id);
        }

        [TestMethod]
        public virtual void ItFecthesCategoryLevelMapping()
        {
            // create entity
            var entity1 = App.Factory.Create<Category>(OnCreateOverrides);
            var entity2 = App.Factory.Create<Category>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.ParentCategoryId = entity1.Id;
            });
            var entity3 = App.Factory.Create<Category>(item =>
            {
                OnCreateOverrides?.Invoke(item);
                item.ParentCategoryId = entity2.Id;
            });

            // check entity fetched
            var mappingFetched = ((CategoryService)serviceIdManageable).GetCategoryLevelMapping(user.Id);
            mappingFetched.Should().ContainKey(entity1.Id);
            mappingFetched[entity1.Id].ShouldBeEquivalentTo(0);
            mappingFetched.Should().ContainKey(entity2.Id);
            mappingFetched[entity2.Id].ShouldBeEquivalentTo(1);
            mappingFetched.Should().ContainKey(entity3.Id);
            mappingFetched[entity3.Id].ShouldBeEquivalentTo(2);
        }

        #region Overrides 

        protected override void ChangeEntity(CategoryModel entity) => entity.Name = "Some other name";

        #endregion
    }
}
