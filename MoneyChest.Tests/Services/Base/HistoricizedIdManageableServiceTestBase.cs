using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities.Base;
using MoneyChest.Data.Entities.History;
using MoneyChest.Model.Converters;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    public abstract class HistoricizedIdManageableServiceTestBase<T, TModel, TConverter, TService, THistory> : HistoricizedEntityModelServiceTestBase<T, TModel, TConverter, TService, THistory>
        where T : class, IHasId, new()
        where TModel : class, IHasId, new()
        where TConverter : IEntityModelConverter<T, TModel>, new()
        where TService : BaseHistoricizedIdManageableService<T, TModel, TConverter>
        where THistory : class, IUserActionHistory, new()
    {
        protected BaseHistoricizedIdManageableService<T, TModel, TConverter> serviceIdManageable;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            serviceIdManageable = (TService)service;
        }

        [TestMethod]
        public virtual void ItFetchesEntityById()
        {
            // create entity
            var entity = App.Factory.Create<T>(OnCreateOverrides);
            var model = converter.ToModel(entity);

            // check entity fetched
            var entityFetched = serviceIdManageable.Get(entity.Id);
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, model);
        }

        [TestMethod]
        public virtual void ItFetchesEntityByIds()
        {
            // create entity
            var entity = App.Factory.Create<T>(OnCreateOverrides);
            var model = converter.ToModel(entity);

            // check entity fetched
            var entityFetched = serviceIdManageable.Get(new List<int>() { entity.Id });
            entityFetched.Should().NotBeNull();
            entityFetched.Count.ShouldBeEquivalentTo(1);
            CheckAreEquivalent(entityFetched[0], model);
        }

        [TestMethod]
        public virtual void ItRemovesEntityById()
        {
            var entity = App.Factory.Create<T>(OnCreateOverrides);
            var model = converter.ToModel(entity);
            serviceIdManageable.Delete(entity.Id);
            service.SaveChanges();

            // check entity removed
            var entityRemoved = FetchItem(model);
            entityRemoved.Should().BeNull();
            OnEntityRemoved(entity);
        }

        protected override TModel FetchItem(TModel model) => serviceIdManageable.Get(model.Id);
        protected override T GetDbItem(TModel model) => Scope.FirstOrDefault(e => e.Id == model.Id);
        protected override T GetDbItem(T entity) => Scope.FirstOrDefault(e => e.Id == entity.Id);
    }
}
