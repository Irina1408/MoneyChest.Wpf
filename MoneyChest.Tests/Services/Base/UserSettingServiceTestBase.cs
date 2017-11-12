using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Base;
using MoneyChest.Services.Converters;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    public abstract class UserSettingServiceTestBase<T, TModel, TConverter, TService> : IntegrationTestBase
        where T : class, IHasUserId, new()
        where TModel : class, IHasUserId
        where TConverter : IEntityModelConverter<T, TModel>, new()
        where TService : UserSettingServiceBase<T, TModel, TConverter>
    {
        protected UserSettingServiceBase<T, TModel, TConverter> service;
        protected IEntityModelConverter<T, TModel> converter;
        protected UserModel user;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            service = (TService)Activator.CreateInstance(typeof(TService), App.Db);
            converter = new TConverter();
            var userConverter = new UserConverter();
            user = userConverter.ToModel(App.Factory.Create<User>());
        }

        [TestMethod]
        public virtual void ItFetchesEntityForUser()
        {
            // create entity
            var entity = converter.ToModel(CreateNew);

            // check entity fetched
            var entityFetched = service.GetForUser(user.Id);
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, entity);
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
            var entitySaved = service.GetForUser(user.Id);
            entitySaved.Should().NotBeNull();
            CheckAreEquivalent(entitySaved, model);
        }

        protected virtual void CheckAreEquivalent(TModel entity1, TModel entity2)
        {
            var entityProperies = typeof(TModel).GetProperties();

            foreach (var prop in entityProperies)
            {
                if (prop.PropertyType == typeof(string) || !prop.PropertyType.IsClass)
                    prop.GetValue(entity1).ShouldBeEquivalentTo(prop.GetValue(entity2));
            }
        }

        protected abstract void ChangeEntity(TModel entity);

        protected virtual Action<T> OnCreateOverrides => item => SetUserId(item, user.Id);
        protected virtual void SetUserId(T entity, int userId) => entity.UserId = userId;
        protected virtual DbSet<T> Entities => App.Db.Set<T>();
        protected virtual IQueryable<T> Scope => Entities;
        protected virtual T CreateNew => GetDbItem(App.Factory.Create<T>(OnCreateOverrides));
        protected virtual T GetDbItem(T entity) => Scope.FirstOrDefault(e => e.UserId == entity.UserId);
    }
}
