using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests
{
    public class UserableIntegrationTestBase : IntegrationTestBase
    {
        protected User user;

        [TestInitialize]
        public override void Init()
        {
            base.Init();
            user = App.Factory.Create<User>();
        }
    }
}
