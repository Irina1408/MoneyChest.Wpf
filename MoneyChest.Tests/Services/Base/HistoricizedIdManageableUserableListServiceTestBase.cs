using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Model.Base;
using MoneyChest.Data.Entities.History;
using MoneyChest.Data.Converters;
using MoneyChest.Services.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Services
{
    public abstract class HistoricizedIdManageableUserableListServiceTestBase<T, TModel, TConverter, TService, THistory> : HistoricizedIdManageableServiceTestBase<T, TModel, TConverter, TService, THistory>
        where T : class, IHasId, IHasUserId, new()
        where TModel : class, IHasId, IHasUserId, new()
        where TConverter : IEntityModelConverter<T, TModel>, new()
        where TService : BaseHistoricizedIdManageableUserableListService<T, TModel, TConverter>
        where THistory : class, IUserActionHistory, new()
    {
        protected BaseHistoricizedIdManageableUserableListService<T, TModel, TConverter> serviceUserableList;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            serviceUserableList = (TService)service;
        }
        
        [TestMethod]
        public virtual void ItFetchesAllEntitiesForUser()
        {
            var entities = new List<TModel>();

            // create entities
            for (int i = 0; i < CountEntitiesForUser; i++)
                entities.Add(converter.ToModel(CreateNew));

            // check entities fetched
            foreach (var entity in entities)
                FetchItem(entity).Should().NotBeNull();
        }

        protected virtual int CountEntitiesForUser => 2;

        protected override void SetUserId(T entity, int userId) => entity.UserId = userId;
        protected override void SetUserId(TModel entity, int userId) => entity.UserId = userId;
    }
}
