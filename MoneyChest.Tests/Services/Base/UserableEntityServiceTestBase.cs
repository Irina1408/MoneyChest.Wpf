using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Base;
using MoneyChest.Data.Converters;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    public abstract class UserableEntityServiceTestBase<T, TModel, TConverter, TService> : UserableIntegrationTestBase
        where T : class, new()
        where TModel : class, new()
        where TConverter : IEntityModelConverter<T, TModel>, new()
        where TService : BaseService<T, TModel, TConverter>
    {
        protected BaseService<T, TModel, TConverter> service;
        protected IEntityModelConverter<T, TModel> converter;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            service = (TService)Activator.CreateInstance(typeof(TService), App.Db);
            converter = new TConverter();
        }

        [TestMethod]
        public virtual void ItAddsEntity()
        {
            var model = App.Factory.CreateInstance<TModel>(OnCreateModelOverrides);
            model = service.Add(model);
            service.SaveChanges();

            // check entity exists
            var ent = FetchItem(model);
            ent.Should().NotBeNull();
            CheckAreEquivalent(ent, model);
            OnEntityAdded(GetDbItem(model));
        }

        [TestMethod]
        public virtual void ItUpdatesEntity()
        {
            var entity = CreateNew;
            var model = converter.ToModel(entity);
            ChangeEntity(model);
            model = service.Update(model);
            service.SaveChanges();

            // check entity changes saved
            var entitySaved = FetchItem(model);
            entitySaved.Should().NotBeNull();
            CheckAreEquivalent(entitySaved, model);
            OnEntityUpdated(entity);
        }

        [TestMethod]
        public virtual void ItRemovesEntity()
        {
            var entity = CreateNew;
            var model = converter.ToModel(entity);
            service.Delete(model);
            service.SaveChanges();

            // check entity removed
            var entityRemoved = FetchItem(model);
            entityRemoved.Should().BeNull();
            OnEntityRemoved(entity);
        }

        #region Protected methods

        protected virtual T CreateNew => GetDbItem(App.Factory.Create<T>(OnCreateOverrides));
        protected virtual Action<T> OnCreateOverrides => item => SetUserId(item, user.Id);
        protected virtual Action<TModel> OnCreateModelOverrides => item => SetUserId(item, user.Id);
        protected virtual DbSet<T> Entities => App.Db.Set<T>();
        protected virtual IQueryable<T> Scope => Entities;
        protected virtual void CheckAreEquivalent(TModel entity1, TModel entity2)
        {
            var entityProperies = typeof(TModel).GetProperties();

            foreach (var prop in entityProperies)
            {
                if (prop.PropertyType == typeof(string) || !prop.PropertyType.IsClass)
                    prop.GetValue(entity1).ShouldBeEquivalentTo(prop.GetValue(entity2));
            }
        }

        protected abstract T GetDbItem(TModel model);
        protected abstract T GetDbItem(T entity);
        protected abstract TModel FetchItem(TModel model);
        protected abstract void ChangeEntity(TModel entity);
        protected abstract void SetUserId(T entity, int userId);
        protected abstract void SetUserId(TModel entity, int userId);

        protected virtual void OnEntityAdded(T entity) { }
        protected virtual void OnEntityUpdated(T entity) { }
        protected virtual void OnEntityRemoved(T entity) { }

        #endregion
    }
}
