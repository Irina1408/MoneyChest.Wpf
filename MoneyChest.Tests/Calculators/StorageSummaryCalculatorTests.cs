using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Calculation.Calculators;
using MoneyChest.Data.Mock;
using MoneyChest.Services.Services;

namespace MoneyChest.Tests.Calculators
{
    [TestClass]
    public class StorageSummaryCalculatorTests : UserableIntegrationTestBase
    {
        [TestMethod]
        public virtual void ItCalculatesStorageSummary()
        {
            // create entity
            var entity = App.Factory.CreateStorage(user.Id, item => item.Value = 1000);
            var service = new StorageService(App.Db);
            var storageSummaryCalculator = new StorageSummaryCalculator(service, user.Id);

            var summary = storageSummaryCalculator.CalculateSummary();
            summary.Items.Count.ShouldBeEquivalentTo(1);
            summary.Items[0].Value.ShouldBeEquivalentTo(entity.Value);
            summary.Items[0].CurrencyId.ShouldBeEquivalentTo(entity.CurrencyId);
            summary.Items[0].Special.ShouldBeEquivalentTo(entity.StorageGroupId);
        }
    }
}
