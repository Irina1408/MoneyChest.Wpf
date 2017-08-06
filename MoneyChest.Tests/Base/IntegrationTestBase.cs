using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Data.Entities;
using MoneyChest.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests
{
    public class IntegrationTestBase
    {
        protected ApplicationFixture App;
        protected User user;

        [TestInitialize]
        public void Init()
        {
            App = new ApplicationFixture();
            user = App.Factory.Create<User>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            App.Dispose();
        }
    }
}
