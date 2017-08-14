﻿using System;
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

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class CalendarSettingServiceTests : UserableEntityServiceTestBase<CalendarSetting, CalendarSettingService>
    {
        #region Overrides

        protected override void ChangeEntity(CalendarSetting entity) => entity.ShowLimits = !entity.ShowLimits;
        protected override void SetUserId(CalendarSetting entity, int userId) => entity.UserId = userId;
        protected override int CountEntitiesForUser => 1;

        #endregion
    }
}