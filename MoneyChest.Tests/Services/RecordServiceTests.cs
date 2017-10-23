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
using MoneyChest.Services.Services.Settings;
using MoneyChest.Model.Model;
using MoneyChest.Model.Converters;
using System.Data.Entity;

namespace MoneyChest.Tests.Services
{
    [TestClass]
    public class RecordServiceTests : HistoricizedIdManageableUserableListServiceTestBase<Record, RecordModel, RecordConverter, RecordService, RecordHistory>
    {
        [TestMethod]
        public void ItFecthesExpeseWithoutCategoryRecordsForPeriodFilterTypes()
        {
            var peridFilterTypes = Enum.GetValues(typeof(PeriodFilterType));

            foreach (PeriodFilterType periodFilterType in peridFilterTypes)
            {
                if (periodFilterType == PeriodFilterType.All || periodFilterType == PeriodFilterType.CustomPeriod)
                    continue;

                var records = CreateRecords(periodFilterType, TransactionType.Expense, _ => _.CategoryId = null);
                CheckRecordExistance(records, periodFilterType, TransactionType.Expense, true);
            }
        }

        [TestMethod]
        public void ItFecthesIncomeWithoutCategoryRecordsForPeriodFilterTypes()
        {
            var peridFilterTypes = Enum.GetValues(typeof(PeriodFilterType));

            foreach (PeriodFilterType periodFilterType in peridFilterTypes)
            {
                if (periodFilterType == PeriodFilterType.All || periodFilterType == PeriodFilterType.CustomPeriod)
                    continue;

                var records = CreateRecords(periodFilterType, TransactionType.Income, _ => _.CategoryId = null);
                CheckRecordExistance(records, periodFilterType, TransactionType.Income, true);
            }
        }

        [TestMethod]
        public void ItFecthesExpeseWithCategoryRecordsForPeriodFilterTypes()
        {
            var category = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var peridFilterTypes = Enum.GetValues(typeof(PeriodFilterType));

            foreach (PeriodFilterType periodFilterType in peridFilterTypes)
            {
                if (periodFilterType == PeriodFilterType.All || periodFilterType == PeriodFilterType.CustomPeriod)
                    continue;

                var records = CreateRecords(periodFilterType, TransactionType.Expense, _ => _.CategoryId = category.Id);
                CheckRecordExistance(records, periodFilterType, TransactionType.Expense, false, new List<int>(new [] { category.Id }));
            }
        }

        [TestMethod]
        public void ItFecthesIncomeWithCategoryRecordsForPeriodFilterTypes()
        {
            var category = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var peridFilterTypes = Enum.GetValues(typeof(PeriodFilterType));

            foreach (PeriodFilterType periodFilterType in peridFilterTypes)
            {
                if (periodFilterType == PeriodFilterType.All || periodFilterType == PeriodFilterType.CustomPeriod)
                    continue;

                var records = CreateRecords(periodFilterType, TransactionType.Income, _ => _.CategoryId = category.Id);
                CheckRecordExistance(records, periodFilterType, TransactionType.Income, true, new List<int>(new[] { category.Id }));
            }
        }

        [TestMethod]
        public void ItFecthesAllExpeseWithoutCategoryRecords()
        {
            var records = CreateRecords(PeriodFilterType.All, TransactionType.Expense, _ => _.CategoryId = null);
            CheckRecordExistance(records, PeriodFilterType.All, TransactionType.Expense, true);
        }

        [TestMethod]
        public void ItFecthesAllIncomeWithoutCategoryRecords()
        {
            var records = CreateRecords(PeriodFilterType.All, TransactionType.Income, _ => _.CategoryId = null);
            CheckRecordExistance(records, PeriodFilterType.All, TransactionType.Income, true);
        }

        [TestMethod]
        public void ItFecthesAllExpeseWithCategoryRecords()
        {
            var category = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var records = CreateRecords(PeriodFilterType.All, TransactionType.Expense, _ => _.CategoryId = category.Id);
            CheckRecordExistance(records, PeriodFilterType.All, TransactionType.Expense, false, new List<int>(new[] { category.Id }));
        }

        [TestMethod]
        public void ItFecthesAllIncomeWithCategoryRecords()
        {
            var category = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var records = CreateRecords(PeriodFilterType.All, TransactionType.Income, _ => _.CategoryId = category.Id);
            CheckRecordExistance(records, PeriodFilterType.All, TransactionType.Income, false, new List<int>(new[] { category.Id }));
        }

        [TestMethod]
        public void ItFecthesCustomPeriodExpeseWithCategoryRecords()
        {
            var rand = new Random();
            DateTime from = DateTime.Today.AddDays(-rand.Next(365));
            DateTime until = DateTime.Today.AddDays(rand.Next(365));

            var category = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var records = App.Factory.CreateRecords(user.Id, from, until, record =>
            {
                record.TransactionType = TransactionType.Expense;
                record.CategoryId = category.Id;
            });

            CheckRecordExistance(records, from, until, TransactionType.Expense, false, new List<int>(new[] { category.Id }));
        }

        [TestMethod]
        public void ItFecthesCustomPeriodIncomeWithCategoryRecords()
        {
            var rand = new Random();
            DateTime from = DateTime.Today.AddDays(-rand.Next(365));
            DateTime until = DateTime.Today.AddDays(rand.Next(365));

            var category = App.Factory.Create<Category>(_ => _.UserId = user.Id);
            var records = App.Factory.CreateRecords(user.Id, from, until, record =>
            {
                record.TransactionType = TransactionType.Income;
                record.CategoryId = category.Id;
            });

            CheckRecordExistance(records, from, until, TransactionType.Income, false, new List<int>(new[] { category.Id }));
        }

        [TestMethod]
        public void ItFecthesCustomPeriodExpeseWithoutCategoryRecords()
        {
            var rand = new Random();
            DateTime from = DateTime.Today.AddDays(-rand.Next(365));
            DateTime until = DateTime.Today.AddDays(rand.Next(365));
            
            var records = App.Factory.CreateRecords(user.Id, from, until, record =>
            {
                record.TransactionType = TransactionType.Expense;
                record.CategoryId = null;
            });

            CheckRecordExistance(records, from, until, TransactionType.Expense, true);
        }

        [TestMethod]
        public void ItFecthesCustomPeriodIncomeWithoutCategoryRecords()
        {
            var rand = new Random();
            DateTime from = DateTime.Today.AddDays(-rand.Next(365));
            DateTime until = DateTime.Today.AddDays(rand.Next(365));

            var records = App.Factory.CreateRecords(user.Id, from, until, record =>
            {
                record.TransactionType = TransactionType.Income;
                record.CategoryId = null;
            });

            CheckRecordExistance(records, from, until, TransactionType.Income, true);
        }

        #region Private helper methods

        private List<Record> CreateRecords(PeriodFilterType period, TransactionType transactionType, Action<Record> overrides = null)
        {
            var settingsService = new GeneralSettingService(App.Db);
            var firstDayOfWeek = settingsService.GetForUser(user.Id).FirstDayOfWeek;

            DateTime from, until;
            int tmp1, tmp2;

            switch (period)
            {
                case PeriodFilterType.Today:
                    from = DateTime.Today;
                    until = DateTime.Today.AddDays(1).AddMinutes(-1);
                    break;

                case PeriodFilterType.Yesterday:
                    from = DateTime.Today.AddDays(-1);
                    until = DateTime.Today.AddMinutes(-1);
                    break;

                case PeriodFilterType.ThisWeek:
                    tmp1 = (int)DateTime.Today.DayOfWeek;
                    tmp2 = 0;
                    while (tmp1 != (int)firstDayOfWeek)
                    {
                        tmp1--; tmp2++;
                    }

                    from = DateTime.Today.AddDays(-tmp2);
                    until = DateTime.Today.AddDays(7 -tmp2).AddMinutes(-1);
                    break;

                case PeriodFilterType.PreviousWeek:
                    tmp1 = (int)DateTime.Today.DayOfWeek;
                    tmp2 = 0;
                    while (tmp1 != (int)firstDayOfWeek)
                    {
                        tmp1--; tmp2++;
                    }

                    from = DateTime.Today.AddDays(-tmp2 - 7);
                    until = DateTime.Today.AddDays(-tmp2).AddMinutes(-1);
                    break;

                case PeriodFilterType.ThisMonth:
                    from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                    until = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month)).AddDays(1).AddMinutes(-1);
                    break;

                case PeriodFilterType.PreviousMonth:
                    tmp1 = DateTime.Today.Month > 1 ? DateTime.Today.Year : DateTime.Today.Year - 1;
                    tmp2 = DateTime.Today.Month > 1 ? DateTime.Today.Month - 1 : 12;

                    from = new DateTime(tmp1, tmp2, 1);
                    until = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMinutes(-1);
                    break;

                case PeriodFilterType.ThisYear:
                    from = new DateTime(DateTime.Today.Year, 1, 1);
                    until = new DateTime(DateTime.Today.Year + 1, 1, 1).AddMinutes(-1);
                    break;

                case PeriodFilterType.PreviousYear:
                    from = new DateTime(DateTime.Today.Year - 1, 1, 1);
                    until = new DateTime(DateTime.Today.Year, 1, 1).AddMinutes(-1);
                    break;

                default:
                    var rand = new Random();
                    from = DateTime.Today.AddDays(-rand.Next(365));
                    until = DateTime.Today.AddDays(rand.Next(365));
                    break;
            }
            
            return App.Factory.CreateRecords(user.Id, from, until, record =>
            {
                record.TransactionType = transactionType;
                overrides?.Invoke(record);
            });
        }

        private void CheckRecordExistance(List<Record> records, PeriodFilterType period, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            var service = (IRecordService)serviceIdManageable;
            var allRecords = service.GetListForUser(user.Id);

            var entityFetched = service.Get(user.Id, period, transactionType, includeWithoutCategory, categoryIds);
            //entityFetched.Count.ShouldBeEquivalentTo(records.Count);

            foreach(var record in records)
                entityFetched.FirstOrDefault(_ => _.Id == record.Id).Should().NotBeNull();
        }
        
        private void CheckRecordExistance(List<Record> records, DateTime from, DateTime until, TransactionType transactionType, bool includeWithoutCategory, List<int> categoryIds = null)
        {
            var service = (IRecordService)serviceIdManageable;

            var entityFetched = service.Get(user.Id, from, until, transactionType, includeWithoutCategory, categoryIds);
            entityFetched.Count.ShouldBeEquivalentTo(records.Count);

            foreach (var record in records)
                entityFetched.FirstOrDefault(_ => _.Id == record.Id).Should().NotBeNull();
        }

        #endregion

        #region Overrides 

        protected override IQueryable<Record> Scope => Entities.Include(_ => _.Currency).Include(_ => _.Category).Include(_ => _.Storage).Include(_ => _.Debt);
        protected override void ChangeEntity(RecordModel entity) => entity.Description = "Some other Description";
        protected override void SetUserId(Record entity, int userId)
        {
            var category = App.Factory.Create<Category>(item => item.UserId = userId);
            var storage = App.Factory.CreateStorage(userId);
            entity.UserId = userId;
            entity.CurrencyId = storage.CurrencyId;
            entity.StorageId = storage.Id;
            entity.CategoryId = category.Id;
        }
        protected override void SetUserId(RecordModel entity, int userId)
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
