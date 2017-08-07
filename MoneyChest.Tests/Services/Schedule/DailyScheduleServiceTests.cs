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
using MoneyChest.Services.Services.Schedule;

namespace MoneyChest.Tests.Services.Schedule
{
    [TestClass]
    public class DailyScheduleServiceTests : IdManageableUserableHistoricizedServiceTestBase<DailySchedule, DailyScheduleService, DailyScheduleHistory>
    {
        #region Overrides 

        protected override void ChangeEntity(DailySchedule entity) => entity.Period += 2;
        protected override void SetUserId(DailySchedule entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var evnt = App.Factory.CreateSimpleEvent(userId);
            entity.EventId = evnt.Id;
        }

        #endregion
    }
}
