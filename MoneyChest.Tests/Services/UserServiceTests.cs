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

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class UserServiceTests : IdManageableUserableHistoricizedServiceTestBase<User, UserService, UserHistory>
    {
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

        #endregion
    }
}
