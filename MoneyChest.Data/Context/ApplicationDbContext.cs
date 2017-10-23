using MoneyChest.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using MoneyChest.Data.Exceptions;
using System.Data.Entity.Infrastructure;
using MoneyChest.Data.Entities.History;

namespace MoneyChest.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        #region Initialization

        public ApplicationDbContext()
            : base(ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString)
        {
        }

        public ApplicationDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        #endregion

        #region Public methods

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        #endregion

        #region Db sets

        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }
        public virtual DbSet<Evnt> Events { get; set; }
        public virtual DbSet<MoneyTransferEvent> MoneyTransferEvents { get; set; }
        public virtual DbSet<RepayDebtEvent> RepayDebtEvents { get; set; }
        public virtual DbSet<SimpleEvent> SimpleEvents { get; set; }
        public virtual DbSet<CalendarSetting> CalendarSettings { get; set; }
        public virtual DbSet<ForecastSetting> ForecastSettings { get; set; }
        public virtual DbSet<GeneralSetting> GeneralSettings { get; set; }
        public virtual DbSet<RecordsViewFilter> RecordsViewFilters { get; set; }
        public virtual DbSet<ReportSetting> ReportSettings { get; set; }
        public virtual DbSet<DailySchedule> DailySchedules { get; set; }
        public virtual DbSet<MonthlySchedule> MonthlySchedules { get; set; }
        public virtual DbSet<MonthlyScheduleMonth> MonthlyScheduleMonths { get; set; }
        public virtual DbSet<OnceSchedule> OnceSchedules { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<WeeklySchedule> WeeklySchedules { get; set; }
        public virtual DbSet<WeeklyScheduleDayOfWeek> WeeklyScheduleDayOfWeeks { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<Limit> Limits { get; set; }
        public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Storage> Storages { get; set; }
        public virtual DbSet<StorageGroup> StorageGroups { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CurrencyHistory> CurrenciesHistory { get; set; }
        public virtual DbSet<CurrencyExchangeRateHistory> CurrencyExchangeRatesHistory { get; set; }
        public virtual DbSet<EventHistory> EventsHistory { get; set; }
        public virtual DbSet<MoneyTransferEventHistory> MoneyTransferEventsHistory { get; set; }
        public virtual DbSet<RepayDebtEventHistory> RepayDebtEventsHistory { get; set; }
        public virtual DbSet<SimpleEventHistory> SimpleEventsHistory { get; set; }
        public virtual DbSet<DailyScheduleHistory> DailySchedulesHistory { get; set; }
        public virtual DbSet<MonthlyScheduleHistory> MonthlySchedulesHistory { get; set; }
        public virtual DbSet<MonthlyScheduleMonthHistory> MonthlyScheduleMonthsHistory { get; set; }
        public virtual DbSet<OnceScheduleHistory> OnceSchedulesHistory { get; set; }
        public virtual DbSet<ScheduleHistory> ScheduleHistory { get; set; }
        public virtual DbSet<WeeklyScheduleHistory> WeeklySchedulesHistory { get; set; }
        public virtual DbSet<WeeklyScheduleDayOfWeekHistory> WeeklyScheduleDayOfWeeksHistory { get; set; }
        public virtual DbSet<CategoryHistory> CategoriesHistory { get; set; }
        public virtual DbSet<DebtHistory> DebtsHistory { get; set; }
        public virtual DbSet<LimitHistory> LimitsHistory { get; set; }
        public virtual DbSet<MoneyTransferHistory> MoneyTransfersHistory { get; set; }
        public virtual DbSet<RecordHistory> RecordsHistory { get; set; }
        public virtual DbSet<StorageHistory> StoragesHistory { get; set; }
        public virtual DbSet<StorageGroupHistory> StorageGroupsHistory { get; set; }
        public virtual DbSet<UserHistory> UsersHistory { get; set; }

        #endregion

        #region Overrides

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category
            modelBuilder.Entity<Category>()
                .HasMany(e => e.GeneralSettingsAsDebtCategory)
                .WithRequired(e => e.DebtCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.GeneralSettingsAsComission)
                .WithRequired(e => e.ComissionCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.ChildCategories)
                .WithOptional(e => e.ParentCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Limits)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Records)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.SimpleEvents)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete(false);

            // User
            modelBuilder.Entity<User>()
                .HasOptional(e => e.CalendarSettings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.ForecastSettings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.GeneralSettings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.RecordsViewFilter)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.ReportSettings)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            // Currency
            modelBuilder.Entity<Currency>()
                .HasMany(e => e.CurrencyExchangeRateFroms)
                .WithRequired(e => e.CurrencyFrom)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.CurrencyExchangeRateTos)
                .WithRequired(e => e.CurrencyTo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.Debts)
                .WithRequired(e => e.Currency)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.Limits)
                .WithRequired(e => e.Currency)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.Records)
                .WithRequired(e => e.Currency)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.SimpleEvents)
                .WithRequired(e => e.Currency)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Currency>()
                .HasMany(e => e.Storages)
                .WithRequired(e => e.Currency)
                .WillCascadeOnDelete(false);

            // Event
            modelBuilder.Entity<Evnt>()
                .HasMany(e => e.Schedules)
                .WithRequired(e => e.Event)
                .WillCascadeOnDelete(false);

            // MoneyTransfer
            modelBuilder.Entity<MoneyTransfer>()
                .HasRequired(e => e.StorageFrom)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MoneyTransfer>()
                .HasRequired(e => e.StorageTo)
                .WithMany()
                .WillCascadeOnDelete(false);

            // MoneyTransferEvent
            modelBuilder.Entity<MoneyTransferEvent>()
                .HasRequired(e => e.StorageFrom)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MoneyTransferEvent>()
                .HasRequired(e => e.StorageTo)
                .WithMany()
                .WillCascadeOnDelete(false);

            // RepayDebtEvent
            modelBuilder.Entity<RepayDebtEvent>()
                .HasRequired(e => e.Storage)
                .WithMany()
                .WillCascadeOnDelete(false);

            // SimpleEvent
            modelBuilder.Entity<SimpleEvent>()
                .HasRequired(e => e.Storage)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimpleEvent>()
                .HasRequired(e => e.Currency)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SimpleEvent>()
                .HasOptional(e => e.Category)
                .WithMany()
                .WillCascadeOnDelete(false);

            // MonthlyScheduleMonth
            modelBuilder.Entity<MonthlyScheduleMonth>()
                .HasRequired(e => e.MonthlySchedule)
                .WithMany(e => e.MonthlyScheduleMonths)
                .WillCascadeOnDelete(false);

            // WeeklyScheduleDayOfWeek
            modelBuilder.Entity<WeeklyScheduleDayOfWeek>()
                .HasRequired(e => e.WeeklySchedule)
                .WithMany(e => e.WeeklyScheduleDaysOfWeek)
                .WillCascadeOnDelete(false);

            // Record
            modelBuilder.Entity<Record>()
                .HasRequired(e => e.Currency)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Record>()
                .HasOptional(e => e.Debt)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Record>()
                .HasOptional(e => e.Storage)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Record>()
                .HasOptional(e => e.Category)
                .WithMany()
                .WillCascadeOnDelete(false);

            // Storage
            modelBuilder.Entity<Storage>()
                .HasRequired(e => e.Currency)
                .WithMany()
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Storage>()
                .HasMany(e => e.StorageFromMoneyTransferEvents)
                .WithRequired(e => e.StorageFrom)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .HasMany(e => e.StorageToMoneyTransferEvents)
                .WithRequired(e => e.StorageTo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .HasMany(e => e.StorageFromMoneyTransfers)
                .WithRequired(e => e.StorageFrom)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .HasMany(e => e.StorageToMoneyTransfers)
                .WithRequired(e => e.StorageTo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .HasMany(e => e.RepayDebtEvents)
                .WithRequired(e => e.Storage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .HasMany(e => e.Records)
                .WithOptional(e => e.Storage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Storage>()
                .HasMany(e => e.SimpleEvents)
                .WithRequired(e => e.Storage)
                .WillCascadeOnDelete(false);

            // StorageGroup
            modelBuilder.Entity<StorageGroup>()
                .HasMany(e => e.Storages)
                .WithRequired(e => e.StorageGroup)
                .WillCascadeOnDelete(false);

            // Debt
            modelBuilder.Entity<Debt>()
                .HasRequired(e => e.Currency)
                .WithMany()
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Debt>()
                .HasOptional(e => e.Storage)
                .WithMany()
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Debt>()
                .HasMany(e => e.RepayDebtEvents)
                .WithRequired(e => e.Debt)
                .WillCascadeOnDelete(false);
        }
        
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                // wrap common exceptions 
                var sqlException = dbUpdateException.InnerException.InnerException as SqlException;

                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 547: throw new ReferenceConstraintException("A constraint reference has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 2627: throw new ViolationOfConstraintException("A constraint violation has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        default: throw;
                    }
                }

                throw;
            }
        }

        public override Task<int> SaveChangesAsync()
        {
            try
            {
                return base.SaveChangesAsync();
            }
            catch (DbUpdateException dbUpdateException)
            {
                // wrap common exceptions 
                SqlException sqlException = dbUpdateException.InnerException.InnerException as SqlException;

                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 547: throw new ReferenceConstraintException("A constraint reference has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 2627: throw new ViolationOfConstraintException("A constraint violation has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        default: throw;
                    }
                }

                throw;
            }
        }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken)
        {
            try
            {
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                // wrap common exceptions 
                SqlException sqlException = dbUpdateException.InnerException.InnerException as SqlException;

                if (sqlException != null)
                {
                    switch (sqlException.Number)
                    {
                        case 547: throw new ReferenceConstraintException("A constraint reference has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        case 2627: throw new ViolationOfConstraintException("A constraint violation has been detected while saving data. Please verify entries and try again.", dbUpdateException);
                        default: throw;
                    }
                }

                throw;
            }
        }


        #endregion
    }
}
