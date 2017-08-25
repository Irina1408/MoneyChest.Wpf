using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Services.Services;
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
            if (CreateUserSettings)
            {
                var userService = new UserService(App.Db);
                user = userService.Add(new User() { Name = "Name", Password = "Password" });
            }
            else
                user = App.Factory.Create<User>();
        }
        
        [TestMethod]
        public virtual void ItFetchesEntityForUser()
        {
            // create entity
            var entity = App.Factory.Create<T>(OnCreateOverrides);

            // check entity fetched
            var entityFetched = FetchItem(entity);
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, entity);
        }

        [TestMethod]
        public virtual void ItFetchesAllEntitiesForUser()
        {
            var entities = new List<T>();

            // create entities
            for (int i = 0; i < CountEntitiesForUser; i++)
                entities.Add(App.Factory.Create<T>(OnCreateOverrides));

            // check entities fetched
            foreach (var entity in entities)
                FetchItem(entity).Should().NotBeNull();
        }

        [TestMethod]
        public virtual void ItAddsEntity()
        {
            var entity = App.Factory.CreateInstance<T>(OnCreateOverrides);
            service.Add(entity);
            service.SaveChanges();

            // check entity exists
            var ent = FetchItem(entity);
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
            var entitySaved = FetchItem(entity);
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
            var entityRemoved = FetchItem(entity);
            entityRemoved.Should().BeNull();
            OnEntityRemoved(entity);
        }

        #region Protected methods

        protected virtual Action<T> OnCreateOverrides => item => SetUserId(item, user.Id);
        protected virtual int GetUserId(T entity) => user.Id;
        protected virtual int CountEntitiesForUser => 2;
        protected virtual bool CreateUserSettings => true;
        protected virtual T FetchItem(T entity) => service.GetForUser(GetUserId(entity));
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
