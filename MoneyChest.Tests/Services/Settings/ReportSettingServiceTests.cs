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
using MoneyChest.Data.Enums;

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class ReportSettingServiceTests : UserableEntityServiceTestBase<ReportSetting, ReportSettingService>
    {
        #region Overrides

        protected override void ChangeEntity(ReportSetting entity) => entity.PeriodFilterType = PeriodFilterType.ThisYear;
        protected override void SetUserId(ReportSetting entity, int userId) => entity.UserId = userId;
        protected override int CountEntitiesForUser => 1;
        protected override bool CreateUserSettings => false;

        #endregion
    }
}
