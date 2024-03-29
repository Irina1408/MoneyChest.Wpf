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
using MoneyChest.Data.Mock;
using MoneyChest.Services.Services.Settings;
using MoneyChest.Model.Enums;
using MoneyChest.Model.Model;
using MoneyChest.Services.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class ReportSettingServiceTests : UserSettingServiceTestBase<ReportSetting, ReportSettingModel, ReportSettingConverter, ReportSettingService>
    {
        #region Overrides

        protected override IQueryable<ReportSetting> Scope => Entities.Include(_ => _.Categories);
        protected override void ChangeEntity(ReportSettingModel entity) => entity.PeriodFilterType = PeriodFilterType.ThisYear;

        #endregion
    }
}
