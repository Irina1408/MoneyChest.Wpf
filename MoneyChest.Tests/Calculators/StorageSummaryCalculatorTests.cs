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
            summary.Keys.FirstOrDefault(_ => _.Id == entity.StorageGroupId).Should().NotBeNull();
            var key = summary.Keys.FirstOrDefault(_ => _.Id == entity.StorageGroupId);
            var summaryItem = summary[key].FirstOrDefault(_ => _.Currency.Id == entity.CurrencyId);
            summaryItem.Should().NotBeNull();
            summaryItem.Value.ShouldBeEquivalentTo(entity.Value);
            summaryItem.Currency.Id.ShouldBeEquivalentTo(entity.CurrencyId);
        }
    }
}
