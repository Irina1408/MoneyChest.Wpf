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
    public class CalendarSettingServiceTests : UserSettingServiceTestBase<CalendarSettings, CalendarSettingsModel, CalendarSettingsConverter, CalendarSettingsService>
    {
        #region Overrides

        protected override IQueryable<CalendarSettings> Scope => Entities.Include(_ => _.StorageGroups);
        protected override void ChangeEntity(CalendarSettingsModel entity) => entity.ShowLimits = !entity.ShowLimits;

        #endregion
    }
}
