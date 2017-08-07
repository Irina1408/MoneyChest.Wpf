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

        [TestInitialize]
        public virtual void Init()
        {
            App = new ApplicationFixture();
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            App.Dispose();
        }
    }
}
