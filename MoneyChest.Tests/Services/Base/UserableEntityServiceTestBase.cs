using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    public abstract class UserableEntityServiceTestBase<T, TService> : IntegrationTestBase
        where T : class, new()
        where TService : BaseUserableService<T>
    {
        protected BaseUserableService<T> service;
        protected User user;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            service = (TService)Activator.CreateInstance(typeof(TService), App.Db);
            user = App.Factory.Create<User>();
        }
        
        [TestMethod]
        public virtual void ItFetchesEntityForUser()
        {
            // create entity
            var entity = App.Factory.Create<T>(OnCreateOverrides);

            // check entity fetched
            var entityFetched= service.GetForUser(GetUserId(entity));
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, entity);
        }

        [TestMethod]
        public virtual void ItFetchesAllEntitiesForUser()
        {
            // create entities
            var entity1 = App.Factory.Create<T>(OnCreateOverrides);
            var entity2 = App.Factory.Create<T>(OnCreateOverrides);

            // check entities fetched
            var entities = service.GetAllForUser(user.Id);
            entities.Count.ShouldBeEquivalentTo(2);
        }

        [TestMethod]
        public virtual void ItAddsEntity()
        {
            var entity = App.Factory.CreateInstance<T>(OnCreateOverrides);
            service.Add(entity);
            service.SaveChanges();

            // check entity exists
            var ent = service.GetForUser(GetUserId(entity));
            ent.Should().NotBeNull();
            CheckAreEquivalent(ent, entity);
            OnEntityAdded(entity);
        }

        [TestMethod]
        public virtual void ItSavesEntityChanges()
        {
            var entity = App.Factory.Create<T>(OnCreateOverrides);
            ChangeEntity(entity);
            service.SaveChanges();

            // check entity changes saved
            var entitySaved = service.GetForUser(GetUserId(entity));
            entitySaved.Should().NotBeNull();
            CheckAreEquivalent(entitySaved, entity);
            OnEntityUpdated(entity);
        }

        [TestMethod]
        public virtual void ItRemovesEntity()
        {
            var entity = App.Factory.Create<T>(OnCreateOverrides);
            service.Delete(entity);
            service.SaveChanges();

            // check entity removed
            var entityRemoved = service.GetForUser(GetUserId(entity));
            entityRemoved.Should().BeNull();
            OnEntityRemoved(entity);
        }

        #region Protected methods

        protected virtual Action<T> OnCreateOverrides => item => SetUserId(item, user.Id);
        protected virtual int GetUserId(T entity) => user.Id;
        protected virtual void CheckAreEquivalent(T entity1, T entity2)
        {
            var entityProperies = typeof(T).GetProperties();

            foreach(var prop in entityProperies)
            {
                if(prop.PropertyType == typeof(string) || !prop.PropertyType.IsClass)
                    prop.GetValue(entity1).ShouldBeEquivalentTo(prop.GetValue(entity2));
            }
        }

        protected abstract void ChangeEntity(T entity);
        protected abstract void SetUserId(T entity, int userId);

        protected virtual void OnEntityAdded(T entity) { }
        protected virtual void OnEntityUpdated(T entity) { }
        protected virtual void OnEntityRemoved(T entity) { }

        #endregion
    }
}
