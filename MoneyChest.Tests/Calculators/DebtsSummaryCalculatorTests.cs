using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoneyChest.Calculation.Calculators;
using MoneyChest.Data.Mock;
using MoneyChest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyChest.Tests.Calculators
{
    [TestClass]
    public class DebtsSummaryCalculatorTests : UserableIntegrationTestBase
    {
        [TestMethod]
        public virtual void ItCalculatesBorrowedDebtsSummary()
        {
            // create entity
            var entity = App.Factory.CreateDebt(user.Id, item =>
            {
                item.DebtType = Data.Enums.DebtType.TakeBorrow;
                item.Value = 1000;
            });
            var service = new DebtService(App.Db);
            var debtsSummaryCalculator = new DebtsSummaryCalculator(service, user.Id);

            var summary = debtsSummaryCalculator.CalculateSummary();
            summary.BorrowedDebts.Count.ShouldBeEquivalentTo(1);
            summary.BorrowedDebts[0].Value.ShouldBeEquivalentTo(entity.Value);
            summary.BorrowedDebts[0].Currency.Id.ShouldBeEquivalentTo(entity.CurrencyId);
        }

        [TestMethod]
        public virtual void ItCalculatesGivenDebtsSummary()
        {
            // create entity
            var entity = App.Factory.CreateDebt(user.Id, item =>
            {
                item.DebtType = Data.Enums.DebtType.GiveBorrow;
                item.Value = 1000;
            });
            var service = new DebtService(App.Db);
            var debtsSummaryCalculator = new DebtsSummaryCalculator(service, user.Id);

            var summary = debtsSummaryCalculator.CalculateSummary();
            summary.GivenDebts.Count.ShouldBeEquivalentTo(1);
            summary.GivenDebts[0].Value.ShouldBeEquivalentTo(entity.Value);
            summary.GivenDebts[0].Currency.Id.ShouldBeEquivalentTo(entity.CurrencyId);
        }
    }
}
