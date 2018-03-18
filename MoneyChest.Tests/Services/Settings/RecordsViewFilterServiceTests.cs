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
using MoneyChest.Data.Mock;
using MoneyChest.Services.Services.Settings;
using MoneyChest.Model.Model;
using System.Data.Entity;
using MoneyChest.Services.Converters;

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class RecordsViewFilterServiceTests : UserSettingServiceTestBase<TransactionsSettings, TransactionsSettingsModel, TransactionsSettingsConverter, TransactionsSettingsService>
    {
        #region Overrides

        protected override IQueryable<RecordsViewFilter> Scope => Entities.Include(_ => _.Categories);
        protected override void ChangeEntity(RecordsViewFilterModel entity) => entity.Description = "Products";

        #endregion
    }
}
