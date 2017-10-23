using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Model.Model;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests
{
    public class UserableIntegrationTestBase : IntegrationTestBase
    {
        protected UserModel user;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            var userService = new UserService(App.Db);
            user = userService.Add(new UserModel() { Name = "Name", Password = "Password" });
        }
    }
}
