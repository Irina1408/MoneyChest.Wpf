namespace MoneyChest.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_transactions_settings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RecordsViewFilterCategories", "RecordsViewFilter_UserId", "dbo.RecordsViewFilter");
            DropForeignKey("dbo.RecordsViewFilterCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.RecordsViewFilter", "UserId", "dbo.Users");
            DropIndex("dbo.RecordsViewFilter", new[] { "UserId" });
            DropIndex("dbo.RecordsViewFilterCategories", new[] { "RecordsViewFilter_UserId" });
            DropIndex("dbo.RecordsViewFilterCategories", new[] { "Category_Id" });
            CreateTable(
                "dbo.DataFilters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsFilterApplied = c.Boolean(nullable: false),
                        TransactionType = c.Int(),
                        Description = c.String(maxLength: 1000),
                        Remark = c.String(maxLength: 4000),
                        AllCategories = c.Boolean(nullable: false),
                        IsSingleCategorySelection = c.Boolean(nullable: false),
                        IncludeWithoutCategory = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TransactionsSettings",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        DataFilterId = c.Int(nullable: false),
                        PeriodFilterId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.PeriodFilters", t => t.PeriodFilterId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.DataFilters", t => t.DataFilterId)
                .Index(t => t.UserId)
                .Index(t => t.DataFilterId)
                .Index(t => t.PeriodFilterId);
            
            CreateTable(
                "dbo.PeriodFilters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeriodType = c.Int(nullable: false),
                        DateFrom = c.DateTime(nullable: false, storeType: "date"),
                        DateUntil = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DataFilterCategories",
                c => new
                    {
                        DataFilter_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DataFilter_Id, t.Category_Id })
                .ForeignKey("dbo.DataFilters", t => t.DataFilter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.Category_Id, cascadeDelete: true)
                .Index(t => t.DataFilter_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.DataFilterStorages",
                c => new
                    {
                        DataFilter_Id = c.Int(nullable: false),
                        Storage_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DataFilter_Id, t.Storage_Id })
                .ForeignKey("dbo.DataFilters", t => t.DataFilter_Id, cascadeDelete: true)
                .ForeignKey("dbo.Storages", t => t.Storage_Id, cascadeDelete: true)
                .Index(t => t.DataFilter_Id)
                .Index(t => t.Storage_Id);
            
            DropTable("dbo.RecordsViewFilter");
            DropTable("dbo.RecordsViewFilterCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.RecordsViewFilterCategories",
                c => new
                    {
                        RecordsViewFilter_UserId = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RecordsViewFilter_UserId, t.Category_Id });
            
            CreateTable(
                "dbo.RecordsViewFilter",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        AllCategories = c.Boolean(nullable: false),
                        Description = c.String(maxLength: 1000),
                        Remark = c.String(maxLength: 4000),
                        PeriodFilterType = c.Int(nullable: false),
                        RecordType = c.Int(),
                        DateFrom = c.DateTime(storeType: "date"),
                        DateUntil = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropForeignKey("dbo.TransactionsSettings", "DataFilterId", "dbo.DataFilters");
            DropForeignKey("dbo.TransactionsSettings", "UserId", "dbo.Users");
            DropForeignKey("dbo.TransactionsSettings", "PeriodFilterId", "dbo.PeriodFilters");
            DropForeignKey("dbo.DataFilterStorages", "Storage_Id", "dbo.Storages");
            DropForeignKey("dbo.DataFilterStorages", "DataFilter_Id", "dbo.DataFilters");
            DropForeignKey("dbo.DataFilterCategories", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.DataFilterCategories", "DataFilter_Id", "dbo.DataFilters");
            DropIndex("dbo.DataFilterStorages", new[] { "Storage_Id" });
            DropIndex("dbo.DataFilterStorages", new[] { "DataFilter_Id" });
            DropIndex("dbo.DataFilterCategories", new[] { "Category_Id" });
            DropIndex("dbo.DataFilterCategories", new[] { "DataFilter_Id" });
            DropIndex("dbo.TransactionsSettings", new[] { "PeriodFilterId" });
            DropIndex("dbo.TransactionsSettings", new[] { "DataFilterId" });
            DropIndex("dbo.TransactionsSettings", new[] { "UserId" });
            DropTable("dbo.DataFilterStorages");
            DropTable("dbo.DataFilterCategories");
            DropTable("dbo.PeriodFilters");
            DropTable("dbo.TransactionsSettings");
            DropTable("dbo.DataFilters");
            CreateIndex("dbo.RecordsViewFilterCategories", "Category_Id");
            CreateIndex("dbo.RecordsViewFilterCategories", "RecordsViewFilter_UserId");
            CreateIndex("dbo.RecordsViewFilter", "UserId");
            AddForeignKey("dbo.RecordsViewFilter", "UserId", "dbo.Users", "Id");
            AddForeignKey("dbo.RecordsViewFilterCategories", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.RecordsViewFilterCategories", "RecordsViewFilter_UserId", "dbo.RecordsViewFilter", "UserId", cascadeDelete: true);
        }
    }
}
