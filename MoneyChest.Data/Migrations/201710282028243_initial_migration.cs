namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CalendarSettings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        PeriodType = c.Int(nullable: false),
                        ShowLimits = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StorageGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsHidden = c.Boolean(nullable: false),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        StorageGroupId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.StorageGroups", t => t.StorageGroupId)
                .Index(t => t.CurrencyId)
                .Index(t => t.StorageGroupId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 10),
                        Symbol = c.String(nullable: false, maxLength: 10),
                        IsUsed = c.Boolean(nullable: false),
                        IsMain = c.Boolean(nullable: false),
                        SymbolAlignmentIsRight = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CurrencyExchangeRates",
                c => new
                    {
                        CurrencyFromId = c.Int(nullable: false),
                        CurrencyToId = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.CurrencyFromId, t.CurrencyToId })
                .ForeignKey("dbo.Currencies", t => t.CurrencyFromId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyToId)
                .Index(t => t.CurrencyFromId)
                .Index(t => t.CurrencyToId);
            
            CreateTable(
                "dbo.Debts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        DebtType = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsRepaid = c.Boolean(nullable: false),
                        TakingDate = c.DateTime(nullable: false, storeType: "date"),
                        RepayingDate = c.DateTime(storeType: "date"),
                        DueDate = c.DateTime(storeType: "date"),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        StorageId = c.Int(),
                        UserId = c.Int(nullable: false),
                        Category_Id = c.Int(),
                        Storage_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Storages", t => t.StorageId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.Storages", t => t.Storage_Id)
                .Index(t => t.CurrencyId)
                .Index(t => t.CategoryId)
                .Index(t => t.StorageId)
                .Index(t => t.UserId)
                .Index(t => t.Category_Id)
                .Index(t => t.Storage_Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        TransactionType = c.Int(),
                        InHistory = c.Boolean(nullable: false),
                        ParentCategoryId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.ParentCategoryId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ParentCategoryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ForecastSettings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        AllCategories = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        FirstUsageDate = c.DateTime(nullable: false),
                        LastUsageDate = c.DateTime(nullable: false),
                        LastSynchronizationDate = c.DateTime(),
                        ServerUserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        TransactionType = c.Int(),
                        InHistory = c.Boolean(nullable: false),
                        ParentCategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.CurrencyHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        Code = c.String(),
                        Symbol = c.String(),
                        IsUsed = c.Boolean(nullable: false),
                        IsMain = c.Boolean(nullable: false),
                        SymbolAlignmentIsRight = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DebtHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                        DebtType = c.Int(nullable: false),
                        TakingDate = c.DateTime(nullable: false, storeType: "date"),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsRepaid = c.Boolean(nullable: false),
                        RepayingDate = c.DateTime(storeType: "date"),
                        DueDate = c.DateTime(storeType: "date"),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        StorageId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EventState = c.Int(nullable: false),
                        PausedToDate = c.DateTime(storeType: "date"),
                        AutoExecution = c.Boolean(nullable: false),
                        AutoExecutionTime = c.Time(precision: 7),
                        ConfirmBeforeExecute = c.Boolean(nullable: false),
                        EventType = c.Int(nullable: false),
                        Remark = c.String(),
                        UserId = c.Int(nullable: false),
                        TakeExistingCurrencyExchangeRate = c.Boolean(),
                        CurrencyExchangeRate = c.Decimal(precision: 18, scale: 2),
                        Commission = c.Decimal(precision: 18, scale: 2),
                        TakeComissionFromReceiver = c.Boolean(),
                        CommissionType = c.Int(),
                        StorageFromId = c.Int(),
                        StorageToId = c.Int(),
                        StorageId = c.Int(),
                        DebtId = c.Int(),
                        TransactionType = c.Int(),
                        CurrencyId = c.Int(),
                        CategoryId = c.Int(),
                        StorageId1 = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Debts", t => t.DebtId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.Storages", t => t.StorageId)
                .ForeignKey("dbo.Storages", t => t.StorageId1)
                .ForeignKey("dbo.Storages", t => t.StorageFromId)
                .ForeignKey("dbo.Storages", t => t.StorageToId)
                .Index(t => t.UserId)
                .Index(t => t.StorageFromId)
                .Index(t => t.StorageToId)
                .Index(t => t.StorageId)
                .Index(t => t.DebtId)
                .Index(t => t.CurrencyId)
                .Index(t => t.CategoryId)
                .Index(t => t.StorageId1);
            
            CreateTable(
                "dbo.Schedules",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleType = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        DateFrom = c.DateTime(storeType: "date"),
                        DateUntil = c.DateTime(storeType: "date"),
                        Period = c.Int(),
                        DateFrom1 = c.DateTime(storeType: "date"),
                        DateUntil1 = c.DateTime(storeType: "date"),
                        DayOfMonth = c.Int(),
                        Date = c.DateTime(storeType: "date"),
                        DateFrom2 = c.DateTime(storeType: "date"),
                        DateUntil2 = c.DateTime(storeType: "date"),
                        Period1 = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Evnt_Id = c.Int(),
                        Evnt_Id1 = c.Int(),
                        Evnt_Id2 = c.Int(),
                        Evnt_Id3 = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .ForeignKey("dbo.Events", t => t.Evnt_Id)
                .ForeignKey("dbo.Events", t => t.Evnt_Id1)
                .ForeignKey("dbo.Events", t => t.Evnt_Id2)
                .ForeignKey("dbo.Events", t => t.Evnt_Id3)
                .Index(t => t.EventId)
                .Index(t => t.Evnt_Id)
                .Index(t => t.Evnt_Id1)
                .Index(t => t.Evnt_Id2)
                .Index(t => t.Evnt_Id3);
            
            CreateTable(
                "dbo.MonthlyScheduleMonths",
                c => new
                    {
                        MonthlyScheduleId = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MonthlyScheduleId, t.Month })
                .ForeignKey("dbo.Schedules", t => t.MonthlyScheduleId)
                .Index(t => t.MonthlyScheduleId);
            
            CreateTable(
                "dbo.WeeklyScheduleDayOfWeeks",
                c => new
                    {
                        WeeklyScheduleId = c.Int(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.WeeklyScheduleId, t.DayOfWeek })
                .ForeignKey("dbo.Schedules", t => t.WeeklyScheduleId)
                .Index(t => t.WeeklyScheduleId);
            
            CreateTable(
                "dbo.EventHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EventState = c.Int(nullable: false),
                        PausedToDate = c.DateTime(storeType: "date"),
                        EventType = c.Int(nullable: false),
                        AutoExecution = c.Boolean(nullable: false),
                        AutoExecutionTime = c.Time(precision: 7),
                        ConfirmBeforeExecute = c.Boolean(nullable: false),
                        Remark = c.String(),
                        TakeExistingCurrencyExchangeRate = c.Boolean(),
                        CurrencyExchangeRate = c.Decimal(precision: 18, scale: 2),
                        Commission = c.Decimal(precision: 18, scale: 2),
                        TakeComissionFromReceiver = c.Boolean(),
                        CommissionType = c.Int(),
                        StorageFromId = c.Int(),
                        StorageToId = c.Int(),
                        StorageId = c.Int(),
                        DebtId = c.Int(),
                        TransactionType = c.Int(),
                        CurrencyId = c.Int(),
                        CategoryId = c.Int(),
                        StorageId1 = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.GeneralSettings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        Language = c.Int(nullable: false),
                        FirstDayOfWeek = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Limits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateFrom = c.DateTime(nullable: false, storeType: "date"),
                        DateUntil = c.DateTime(nullable: false, storeType: "date"),
                        LimitState = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .Index(t => t.CurrencyId)
                .Index(t => t.CategoryId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LimitHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        DateFrom = c.DateTime(nullable: false, storeType: "date"),
                        DateUntil = c.DateTime(nullable: false, storeType: "date"),
                        LimitState = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SpentValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        TransactionType = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CategoryId = c.Int(),
                        CurrencyId = c.Int(nullable: false),
                        StorageId = c.Int(),
                        DebtId = c.Int(),
                        UserId = c.Int(nullable: false),
                        Debt_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Debts", t => t.DebtId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Debts", t => t.Debt_Id)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId)
                .ForeignKey("dbo.Storages", t => t.StorageId)
                .Index(t => t.CategoryId)
                .Index(t => t.CurrencyId)
                .Index(t => t.StorageId)
                .Index(t => t.DebtId)
                .Index(t => t.UserId)
                .Index(t => t.Debt_Id);
            
            CreateTable(
                "dbo.RecordHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Description = c.String(),
                        TransactionType = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CategoryId = c.Int(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        StorageId = c.Int(),
                        DebtId = c.Int(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RecordsViewFilter",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        AllCategories = c.Boolean(nullable: false),
                        Description = c.String(),
                        Remark = c.String(),
                        PeriodFilterType = c.Int(nullable: false),
                        TransactionType = c.Int(),
                        DateFrom = c.DateTime(storeType: "date"),
                        DateUntil = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ReportSetting",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        IncludeRecordsWithoutCategory = c.Boolean(nullable: false),
                        AllCategories = c.Boolean(nullable: false),
                        ReportType = c.Int(nullable: false),
                        DataType = c.Int(),
                        PeriodFilterType = c.Int(nullable: false),
                        CategoryLevel = c.Int(nullable: false),
                        DateFrom = c.DateTime(storeType: "date"),
                        DateUntil = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StorageGroupHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StorageHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                        IsHidden = c.Boolean(nullable: false),
                        StorageGroupId = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        CurrencyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MoneyTransfers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Commission = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionType = c.Int(),
                        TakeComissionFromReceiver = c.Boolean(nullable: false),
                        Remark = c.String(),
                        StorageFromId = c.Int(nullable: false),
                        StorageToId = c.Int(nullable: false),
                        CategoryId = c.Int(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .ForeignKey("dbo.Storages", t => t.StorageFromId)
                .ForeignKey("dbo.Storages", t => t.StorageToId)
                .Index(t => t.StorageFromId)
                .Index(t => t.StorageToId)
                .Index(t => t.CategoryId)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.CurrencyExchangeRateHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CurrencyFromId = c.Int(nullable: false),
                        CurrencyToId = c.Int(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ScheduleHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        ScheduleType = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                        DateFrom = c.DateTime(storeType: "date"),
                        DateUntil = c.DateTime(storeType: "date"),
                        Period = c.Int(),
                        DateFrom1 = c.DateTime(storeType: "date"),
                        DateUntil1 = c.DateTime(storeType: "date"),
                        DayOfMonth = c.Int(),
                        Date = c.DateTime(storeType: "date"),
                        DateFrom2 = c.DateTime(storeType: "date"),
                        DateUntil2 = c.DateTime(storeType: "date"),
                        Period1 = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MoneyTransferHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrencyExchangeRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Commission = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CommissionType = c.Int(),
                        TakeComissionFromReceiver = c.Boolean(nullable: false),
                        Remark = c.String(),
                        StorageFromId = c.Int(nullable: false),
                        StorageToId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MonthlyScheduleMonthHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        MonthlyScheduleId = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Name = c.String(),
                        Password = c.String(),
                        FirstUsageDate = c.DateTime(nullable: false),
                        LastUsageDate = c.DateTime(nullable: false),
                        LastSynchronizationDate = c.DateTime(),
                        ServerUserId = c.String(),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.WeeklyScheduleDayOfWeekHistories",
                c => new
                    {
                        ActionId = c.Int(nullable: false, identity: true),
                        ActionDateTime = c.DateTime(nullable: false),
                        ActionType = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        WeeklyScheduleId = c.Int(nullable: false),
                        DayOfWeek = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ActionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.StorageGroupCalendarSettings",
                c => new
                    {
                        StorageGroup_Id = c.Int(nullable: false),
                        CalendarSetting_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StorageGroup_Id, t.CalendarSetting_UserId })
                .ForeignKey("dbo.StorageGroups", t => t.StorageGroup_Id, cascadeDelete: true)
                .ForeignKey("dbo.CalendarSettings", t => t.CalendarSetting_UserId, cascadeDelete: true)
                .Index(t => t.StorageGroup_Id)
                .Index(t => t.CalendarSetting_UserId);
            
            CreateTable(
                "dbo.ForecastSettingCategories",
                c => new
                    {
                        ForecastSetting_UserId = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ForecastSetting_UserId, t.Category_Id })
                .ForeignKey("dbo.ForecastSettings", t => t.ForecastSetting_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.ForecastSetting_UserId)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.RecordsViewFilterCategories",
                c => new
                    {
                        RecordsViewFilter_UserId = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecordsViewFilter_UserId, t.Category_Id })
                .ForeignKey("dbo.RecordsViewFilter", t => t.RecordsViewFilter_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.RecordsViewFilter_UserId)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.ReportSettingCategories",
                c => new
                    {
                        ReportSetting_UserId = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ReportSetting_UserId, t.Category_Id })
                .ForeignKey("dbo.ReportSetting", t => t.ReportSetting_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.ReportSetting_UserId)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WeeklyScheduleDayOfWeekHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.ScheduleHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.MonthlyScheduleMonthHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.MoneyTransferHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.CurrencyExchangeRateHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Storages", "StorageGroupId", "dbo.StorageGroups");
            DropForeignKey("dbo.MoneyTransfers", "StorageToId", "dbo.Storages");
            DropForeignKey("dbo.Events", "StorageToId", "dbo.Storages");
            DropForeignKey("dbo.MoneyTransfers", "StorageFromId", "dbo.Storages");
            DropForeignKey("dbo.Events", "StorageFromId", "dbo.Storages");
            DropForeignKey("dbo.Events", "StorageId1", "dbo.Storages");
            DropForeignKey("dbo.Events", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.Records", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.Debts", "Storage_Id", "dbo.Storages");
            DropForeignKey("dbo.Storages", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Events", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Records", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Limits", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Debts", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Debts", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.Events", "DebtId", "dbo.Debts");
            DropForeignKey("dbo.Records", "Debt_Id", "dbo.Debts");
            DropForeignKey("dbo.Debts", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Events", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Records", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.MoneyTransfers", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.MoneyTransfers", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Limits", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.StorageHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Storages", "UserId", "dbo.Users");
            DropForeignKey("dbo.StorageGroupHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.StorageGroups", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReportSetting", "UserId", "dbo.Users");
            DropForeignKey("dbo.ReportSettingCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ReportSettingCategories", "ReportSetting_UserId", "dbo.ReportSetting");
            DropForeignKey("dbo.RecordsViewFilter", "UserId", "dbo.Users");
            DropForeignKey("dbo.RecordsViewFilterCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.RecordsViewFilterCategories", "RecordsViewFilter_UserId", "dbo.RecordsViewFilter");
            DropForeignKey("dbo.RecordHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Records", "UserId", "dbo.Users");
            DropForeignKey("dbo.Records", "DebtId", "dbo.Debts");
            DropForeignKey("dbo.LimitHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Limits", "UserId", "dbo.Users");
            DropForeignKey("dbo.GeneralSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.ForecastSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.EventHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Schedules", "Evnt_Id3", "dbo.Events");
            DropForeignKey("dbo.Events", "UserId", "dbo.Users");
            DropForeignKey("dbo.Schedules", "Evnt_Id2", "dbo.Events");
            DropForeignKey("dbo.Schedules", "Evnt_Id1", "dbo.Events");
            DropForeignKey("dbo.Schedules", "Evnt_Id", "dbo.Events");
            DropForeignKey("dbo.WeeklyScheduleDayOfWeeks", "WeeklyScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.Schedules", "EventId", "dbo.Events");
            DropForeignKey("dbo.MonthlyScheduleMonths", "MonthlyScheduleId", "dbo.Schedules");
            DropForeignKey("dbo.DebtHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Debts", "UserId", "dbo.Users");
            DropForeignKey("dbo.CurrencyHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Currencies", "UserId", "dbo.Users");
            DropForeignKey("dbo.CategoryHistories", "UserId", "dbo.Users");
            DropForeignKey("dbo.Categories", "UserId", "dbo.Users");
            DropForeignKey("dbo.CalendarSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.ForecastSettingCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.ForecastSettingCategories", "ForecastSetting_UserId", "dbo.ForecastSettings");
            DropForeignKey("dbo.Debts", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.Categories", "ParentCategoryId", "dbo.Categories");
            DropForeignKey("dbo.CurrencyExchangeRates", "CurrencyToId", "dbo.Currencies");
            DropForeignKey("dbo.CurrencyExchangeRates", "CurrencyFromId", "dbo.Currencies");
            DropForeignKey("dbo.StorageGroupCalendarSettings", "CalendarSetting_UserId", "dbo.CalendarSettings");
            DropForeignKey("dbo.StorageGroupCalendarSettings", "StorageGroup_Id", "dbo.StorageGroups");
            DropIndex("dbo.ReportSettingCategories", new[] { "Category_Id" });
            DropIndex("dbo.ReportSettingCategories", new[] { "ReportSetting_UserId" });
            DropIndex("dbo.RecordsViewFilterCategories", new[] { "Category_Id" });
            DropIndex("dbo.RecordsViewFilterCategories", new[] { "RecordsViewFilter_UserId" });
            DropIndex("dbo.ForecastSettingCategories", new[] { "Category_Id" });
            DropIndex("dbo.ForecastSettingCategories", new[] { "ForecastSetting_UserId" });
            DropIndex("dbo.StorageGroupCalendarSettings", new[] { "CalendarSetting_UserId" });
            DropIndex("dbo.StorageGroupCalendarSettings", new[] { "StorageGroup_Id" });
            DropIndex("dbo.WeeklyScheduleDayOfWeekHistories", new[] { "UserId" });
            DropIndex("dbo.UserHistories", new[] { "UserId" });
            DropIndex("dbo.MonthlyScheduleMonthHistories", new[] { "UserId" });
            DropIndex("dbo.MoneyTransferHistories", new[] { "UserId" });
            DropIndex("dbo.ScheduleHistories", new[] { "UserId" });
            DropIndex("dbo.CurrencyExchangeRateHistories", new[] { "UserId" });
            DropIndex("dbo.MoneyTransfers", new[] { "Category_Id" });
            DropIndex("dbo.MoneyTransfers", new[] { "CategoryId" });
            DropIndex("dbo.MoneyTransfers", new[] { "StorageToId" });
            DropIndex("dbo.MoneyTransfers", new[] { "StorageFromId" });
            DropIndex("dbo.StorageHistories", new[] { "UserId" });
            DropIndex("dbo.StorageGroupHistories", new[] { "UserId" });
            DropIndex("dbo.ReportSetting", new[] { "UserId" });
            DropIndex("dbo.RecordsViewFilter", new[] { "UserId" });
            DropIndex("dbo.RecordHistories", new[] { "UserId" });
            DropIndex("dbo.Records", new[] { "Debt_Id" });
            DropIndex("dbo.Records", new[] { "UserId" });
            DropIndex("dbo.Records", new[] { "DebtId" });
            DropIndex("dbo.Records", new[] { "StorageId" });
            DropIndex("dbo.Records", new[] { "CurrencyId" });
            DropIndex("dbo.Records", new[] { "CategoryId" });
            DropIndex("dbo.LimitHistories", new[] { "UserId" });
            DropIndex("dbo.Limits", new[] { "UserId" });
            DropIndex("dbo.Limits", new[] { "CategoryId" });
            DropIndex("dbo.Limits", new[] { "CurrencyId" });
            DropIndex("dbo.GeneralSettings", new[] { "UserId" });
            DropIndex("dbo.EventHistories", new[] { "UserId" });
            DropIndex("dbo.WeeklyScheduleDayOfWeeks", new[] { "WeeklyScheduleId" });
            DropIndex("dbo.MonthlyScheduleMonths", new[] { "MonthlyScheduleId" });
            DropIndex("dbo.Schedules", new[] { "Evnt_Id3" });
            DropIndex("dbo.Schedules", new[] { "Evnt_Id2" });
            DropIndex("dbo.Schedules", new[] { "Evnt_Id1" });
            DropIndex("dbo.Schedules", new[] { "Evnt_Id" });
            DropIndex("dbo.Schedules", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "StorageId1" });
            DropIndex("dbo.Events", new[] { "CategoryId" });
            DropIndex("dbo.Events", new[] { "CurrencyId" });
            DropIndex("dbo.Events", new[] { "DebtId" });
            DropIndex("dbo.Events", new[] { "StorageId" });
            DropIndex("dbo.Events", new[] { "StorageToId" });
            DropIndex("dbo.Events", new[] { "StorageFromId" });
            DropIndex("dbo.Events", new[] { "UserId" });
            DropIndex("dbo.DebtHistories", new[] { "UserId" });
            DropIndex("dbo.CurrencyHistories", new[] { "UserId" });
            DropIndex("dbo.CategoryHistories", new[] { "UserId" });
            DropIndex("dbo.ForecastSettings", new[] { "UserId" });
            DropIndex("dbo.Categories", new[] { "UserId" });
            DropIndex("dbo.Categories", new[] { "ParentCategoryId" });
            DropIndex("dbo.Debts", new[] { "Storage_Id" });
            DropIndex("dbo.Debts", new[] { "Category_Id" });
            DropIndex("dbo.Debts", new[] { "UserId" });
            DropIndex("dbo.Debts", new[] { "StorageId" });
            DropIndex("dbo.Debts", new[] { "CategoryId" });
            DropIndex("dbo.Debts", new[] { "CurrencyId" });
            DropIndex("dbo.CurrencyExchangeRates", new[] { "CurrencyToId" });
            DropIndex("dbo.CurrencyExchangeRates", new[] { "CurrencyFromId" });
            DropIndex("dbo.Currencies", new[] { "UserId" });
            DropIndex("dbo.Storages", new[] { "UserId" });
            DropIndex("dbo.Storages", new[] { "StorageGroupId" });
            DropIndex("dbo.Storages", new[] { "CurrencyId" });
            DropIndex("dbo.StorageGroups", new[] { "UserId" });
            DropIndex("dbo.CalendarSettings", new[] { "UserId" });
            DropTable("dbo.ReportSettingCategories");
            DropTable("dbo.RecordsViewFilterCategories");
            DropTable("dbo.ForecastSettingCategories");
            DropTable("dbo.StorageGroupCalendarSettings");
            DropTable("dbo.WeeklyScheduleDayOfWeekHistories");
            DropTable("dbo.UserHistories");
            DropTable("dbo.MonthlyScheduleMonthHistories");
            DropTable("dbo.MoneyTransferHistories");
            DropTable("dbo.ScheduleHistories");
            DropTable("dbo.CurrencyExchangeRateHistories");
            DropTable("dbo.MoneyTransfers");
            DropTable("dbo.StorageHistories");
            DropTable("dbo.StorageGroupHistories");
            DropTable("dbo.ReportSetting");
            DropTable("dbo.RecordsViewFilter");
            DropTable("dbo.RecordHistories");
            DropTable("dbo.Records");
            DropTable("dbo.LimitHistories");
            DropTable("dbo.Limits");
            DropTable("dbo.GeneralSettings");
            DropTable("dbo.EventHistories");
            DropTable("dbo.WeeklyScheduleDayOfWeeks");
            DropTable("dbo.MonthlyScheduleMonths");
            DropTable("dbo.Schedules");
            DropTable("dbo.Events");
            DropTable("dbo.DebtHistories");
            DropTable("dbo.CurrencyHistories");
            DropTable("dbo.CategoryHistories");
            DropTable("dbo.Users");
            DropTable("dbo.ForecastSettings");
            DropTable("dbo.Categories");
            DropTable("dbo.Debts");
            DropTable("dbo.CurrencyExchangeRates");
            DropTable("dbo.Currencies");
            DropTable("dbo.Storages");
            DropTable("dbo.StorageGroups");
            DropTable("dbo.CalendarSettings");
        }
    }
}
