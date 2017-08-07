using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities.History;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    public abstract class IdManageableUserableHistoricizedServiceTestBase<T, TService, THistory> : HistoricizedServiceTestBase<T, TService, THistory>
        where T : class, new()
        where TService : BaseHistoricizedService<T>, IIdManageable<T>
        where THistory : class, IUserActionHistory, new()
    {
        protected IIdManageable<T> serviceIdManageable;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            serviceIdManageable = (IIdManageable<T>)service;
        }

        [TestMethod]
        public virtual void ItFetchesEntityById()
        {
            // create entity
            var entity = App.Factory.Create<T>(OnCreateOverrides);

            // check entity fetched
            var entityFetched = serviceIdManageable.Get((int)entity.GetType().GetProperty(IdPropertyName).GetValue(entity));
            entityFetched.Should().NotBeNull();
            CheckAreEquivalent(entityFetched, entity);
        }

        [TestMethod]
        public virtual void ItRemovesEntityById()
        {
            var entity = App.Factory.Create<T>(OnCreateOverrides);
            serviceIdManageable.Delete((int)entity.GetType().GetProperty(IdPropertyName).GetValue(entity));
            service.SaveChanges();

            // check entity removed
            var entityRemoved = service.GetForUser(GetUserId(entity));
            entityRemoved.Should().BeNull();
            OnEntityRemoved(entity);
        }

        protected virtual string IdPropertyName => "Id";
    }
}
