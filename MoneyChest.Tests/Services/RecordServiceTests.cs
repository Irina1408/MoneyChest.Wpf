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
using MoneyChest.Data.Enums;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class RecordServiceTests : IdManageableUserableHistoricizedServiceTestBase<Record, RecordService, RecordHistory>
    {
        [TestMethod]
        public virtual void ItFecthesTodayExpeseWithWithoutCategoryRecords()
        {
            var service = (IRecordService)serviceIdManageable;
            // create entity
            var entity = App.Factory.CreateRecord(user.Id, item => 
            {
                item.CategoryId = null;
                item.Date = DateTime.Now;
                item.TransactionType = TransactionType.Expense;
            });

            // check entity fetched
            var entityFetched = service.Get(user.Id, PeriodFilterType.Today, TransactionType.Expense, true);
            entityFetched.Count.ShouldBeEquivalentTo(1);
            entityFetched[0].Id.ShouldBeEquivalentTo(entity.Id);
        }

        [TestMethod]
        public virtual void ItFecthesTodayIncomeWithWithoutCategoryRecords()
        {
            var service = (IRecordService)serviceIdManageable;
            // create entity
            var entity = App.Factory.CreateRecord(user.Id, item =>
            {
                item.CategoryId = null;
                item.Date = DateTime.Now;
                item.TransactionType = TransactionType.Income;
            });

            // check entity fetched
            var entityFetched = service.Get(user.Id, PeriodFilterType.Today, TransactionType.Income, true);
            entityFetched.Count.ShouldBeEquivalentTo(1);
            entityFetched[0].Id.ShouldBeEquivalentTo(entity.Id);
        }

        #region Private helper methods

        private List<Record> CreateRecords(PeriodFilterType period, TransactionType transactionType)
        {
            return new List<Record>();
        }

        #endregion

        #region Overrides 

        protected override void ChangeEntity(Record entity) => entity.Description = "Some other Description";
        protected override void SetUserId(Record entity, int userId)
        {
            var category = App.Factory.Create<Category>(item => item.UserId = userId);
            var storage = App.Factory.CreateStorage(userId);
            entity.UserId = userId;
            entity.CurrencyId = storage.CurrencyId;
            entity.StorageId = storage.Id;
            entity.CategoryId = category.Id;
        }

        #endregion
    }
}
