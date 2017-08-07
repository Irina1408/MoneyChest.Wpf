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

namespace MoneyChest.Tests.Services.Settings
{
    [TestClass]
    public class GeneralSettingServiceTests : UserableEntityServiceTestBase<GeneralSetting, GeneralSettingService>
    {
        #region Overrides

        protected override void ChangeEntity(GeneralSetting entity) => entity.HideCoinBoxStorages = !entity.HideCoinBoxStorages;

        protected override void SetUserId(GeneralSetting entity, int userId)
        {
            var category1 = App.Factory.Create<Category>(item => item.UserId = userId);
            var category2 = App.Factory.Create<Category>(item => item.UserId = userId);

            entity.ComissionCategoryId = category1.Id;
            entity.DebtCategoryId = category2.Id;
            entity.UserId = userId;
        }

        #endregion
    }
}
