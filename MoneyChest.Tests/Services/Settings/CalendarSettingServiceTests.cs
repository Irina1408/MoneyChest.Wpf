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
using MoneyChest.Services.Services.Events;
using MoneyChest.Data.Mock;
using MoneyChest.Services.Services.Settings;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class CalendarSettingServiceTests : UserSettingServiceTestBase<CalendarSetting, CalendarSettingModel, CalendarSettingConverter, CalendarSettingService>
    {
        #region Overrides

        protected override IQueryable<CalendarSetting> Scope => Entities.Include(_ => _.StorageGroups);
        protected override void ChangeEntity(CalendarSettingModel entity) => entity.ShowLimits = !entity.ShowLimits;

        #endregion
    }
}
