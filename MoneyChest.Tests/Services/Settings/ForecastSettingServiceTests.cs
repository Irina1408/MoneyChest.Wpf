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
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class ForecastSettingServiceTests : UserSettingServiceTestBase<ForecastSetting, ForecastSettingModel, ForecastSettingConverter, ForecastSettingService>
    {
        #region Overrides

        protected override IQueryable<ForecastSetting> Scope => Entities.Include(_ => _.Categories);
        protected override void ChangeEntity(ForecastSettingModel entity) => entity.RepeatsCount += 10;

        #endregion
    }
}
