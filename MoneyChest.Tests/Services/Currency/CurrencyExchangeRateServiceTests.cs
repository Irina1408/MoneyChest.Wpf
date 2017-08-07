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
    public class CurrencyExchangeRateServiceTests : HistoricizedServiceTestBase<CurrencyExchangeRate, CurrencyExchangeRateService, CurrencyExchangeRateHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(CurrencyExchangeRate entity) => entity.Rate += 5;

        protected override void SetUserId(CurrencyExchangeRate entity, int userId)
        {
            var currency1 = App.Factory.Create<Currency>(item => item.UserId = userId);
            var currency2 = App.Factory.Create<Currency>(item => item.UserId = userId);

            entity.CurrencyFromId = currency1.Id;
            entity.CurrencyToId = currency2.Id;
        }

        #endregion
    }
}
