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
using MoneyChest.Model.Converters;

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class GeneralSettingServiceTests : UserSettingServiceTestBase<GeneralSetting, GeneralSettingModel, GeneralSettingConverter, GeneralSettingService>
    {
        #region Overrides

        protected override void ChangeEntity(GeneralSettingModel entity) => entity.FirstDayOfWeek = DayOfWeek.Thursday;

        #endregion
    }
}
