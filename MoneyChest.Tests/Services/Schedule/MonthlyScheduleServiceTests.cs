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
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services.Schedule
{
    [TestClass]
    public class MonthlyScheduleServiceTests : HistoricizedIdManageableServiceTestBase<MonthlySchedule, MonthlyScheduleModel, MonthlyScheduleConverter, MonthlyScheduleService, MonthlyScheduleHistory>
    {
        #region Overrides 

        protected override IQueryable<MonthlySchedule> Scope => Entities.Include(_ => _.MonthlyScheduleMonths);
        protected override void ChangeEntity(MonthlyScheduleModel entity) => entity.DayOfMonth += 2;
        protected override void SetUserId(MonthlySchedule entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var evnt = App.Factory.CreateSimpleEvent(userId);
            entity.EventId = evnt.Id;
        }
        protected override void SetUserId(MonthlyScheduleModel entity, int userId)
        {
            var storage = App.Factory.CreateStorage(userId);
            var evnt = App.Factory.CreateSimpleEvent(userId);
            entity.EventId = evnt.Id;
        }

        #endregion
    }
}
